using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using ClassLibraryWPCalculator;
using ClassLibraryWhile;

namespace WpfAppWhile
{
    /// <summary>
    /// Основная модель представления для демонстрации работы циклов с верификацией
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private int[] _array;
        private int _currentIndex;
        private int _currentResult;
        private bool _isArrayCreated = false;
        private int _selectedArrayIndex = -1;
        private Brush _currentIndexBackground = Brushes.Transparent;
        private Brush _currentResultBackground = Brushes.Transparent;
        private WPCalculator _wpCalculator = new WPCalculator();

        /// <summary>
        /// Инициализация модели представления с начальными значениями
        /// </summary>
        public MainViewModel()
        {
            ArrayLength = 10;
            MinValue = 0;
            MaxValue = 100;
            Threshold = 50;

            ArrayItems = new ObservableCollection<ArrayItem>();
            GenerateArrayCommand = new RelayCommand(_ => GenerateArray());
            StepCommand = new RelayCommand(_ => ExecuteStep(), _ => CanExecuteStep());
            RunCommand = new RelayCommand(_ => ExecuteRun(), _ => CanExecuteStep());
            ResetCommand = new RelayCommand(_ => ResetExecution());

            IsPrefixSumMode = true;
            UpdateMode();

            StatusMessage = "Создайте массив для начала работы";
            PostCondition = "Массив не создан";
        }

        // Команды управления выполнением алгоритма
        public ICommand GenerateArrayCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand ResetCommand { get; }

        /// <summary>
        /// Проверка возможности выполнения шага алгоритма
        /// </summary>
        private bool CanExecuteStep()
        {
            return _isArrayCreated && _array != null && _array.Length > 0 && _currentIndex < _array.Length;
        }

        // Свойства для параметров массива
        private int _arrayLength;
        public int ArrayLength
        {
            get => _arrayLength;
            set { _arrayLength = value; OnPropertyChanged(nameof(ArrayLength)); }
        }

        private int _minValue;
        public int MinValue
        {
            get => _minValue;
            set { _minValue = value; OnPropertyChanged(nameof(MinValue)); }
        }

        private int _maxValue;
        public int MaxValue
        {
            get => _maxValue;
            set { _maxValue = value; OnPropertyChanged(nameof(MaxValue)); }
        }

        private int _threshold;
        public int Threshold
        {
            get => _threshold;
            set { _threshold = value; OnPropertyChanged(nameof(Threshold)); }
        }

        // Коллекция элементов массива для отображения в интерфейсе
        public ObservableCollection<ArrayItem> ArrayItems { get; }

        // Свойства для визуального выделения элементов
        public int SelectedArrayIndex
        {
            get => _selectedArrayIndex;
            set { _selectedArrayIndex = value; OnPropertyChanged(nameof(SelectedArrayIndex)); }
        }

        public Brush CurrentIndexBackground
        {
            get => _currentIndexBackground;
            set { _currentIndexBackground = value; OnPropertyChanged(nameof(CurrentIndexBackground)); }
        }

        public Brush CurrentResultBackground
        {
            get => _currentResultBackground;
            set { _currentResultBackground = value; OnPropertyChanged(nameof(CurrentResultBackground)); }
        }

        // Свойства режимов работы алгоритма
        private bool _isPrefixSumMode;
        public bool IsPrefixSumMode
        {
            get => _isPrefixSumMode;
            set { _isPrefixSumMode = value; OnPropertyChanged(nameof(IsPrefixSumMode)); if (value) UpdateMode(); }
        }

        private bool _isPrefixMaxMode;
        public bool IsPrefixMaxMode
        {
            get => _isPrefixMaxMode;
            set { _isPrefixMaxMode = value; OnPropertyChanged(nameof(IsPrefixMaxMode)); if (value) UpdateMode(); }
        }

        // Текущее состояние выполнения алгоритма
        public int CurrentIndex
        {
            get => _currentIndex;
            set { _currentIndex = value; OnPropertyChanged(nameof(CurrentIndex)); UpdateState(); }
        }

        public int CurrentResult
        {
            get => _currentResult;
            set { _currentResult = value; OnPropertyChanged(nameof(CurrentResult)); UpdateState(); }
        }

        // Свойства для отображения состояния верификации
        public string InvariantDescription { get; set; }
        public string InvariantFormula { get; set; }
        public string InvariantBeforeStatus { get; set; } = "Не проверено";
        public string InvariantAfterStatus { get; set; } = "Не проверено";
        public Brush InvariantBeforeColor { get; set; } = Brushes.Gray;
        public Brush InvariantAfterColor { get; set; } = Brushes.Gray;

        public string VariantFunction { get; set; }
        public int VariantCurrent { get; set; }
        public int VariantMax { get; set; }
        public string VariantProgressText { get; set; }

        public string WPFormula { get; set; }
        public string WPRussianDescription { get; set; }
        public string WPResult { get; set; } = "Не проверено";
        public Brush WPResultColor { get; set; } = Brushes.Gray;

        public string PostCondition { get; set; }
        public string StatusMessage { get; set; } = "Готов";

        /// <summary>
        /// Генерация нового массива с заданными параметрами
        /// </summary>
        private void GenerateArray()
        {
            try
            {
                if (ArrayLength <= 0)
                {
                    MessageBox.Show("Длина массива должна быть больше 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    OnPropertyChanged(nameof(StatusMessage));
                    return;
                }

                _array = ArrayGenerator.GenerateArray(ArrayLength, MinValue, MaxValue);
                _isArrayCreated = true;
                UpdateArrayItems();
                ResetExecution();
                StatusMessage = $"Массив из {ArrayLength} элементов создан";
                OnPropertyChanged(nameof(StatusMessage));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка создания массива: {ex.Message}";
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        /// <summary>
        /// Обновление коллекции элементов массива для отображения
        /// </summary>
        private void UpdateArrayItems()
        {
            ArrayItems.Clear();
            if (_array == null) return;

            for (int i = 0; i < _array.Length; i++)
            {
                ArrayItems.Add(new ArrayItem
                {
                    Index = i,
                    Value = _array[i],
                    IsCurrentElement = false
                });
            }
        }

        /// <summary>
        /// Обновление визуального выделения текущих элементов
        /// </summary>
        private void UpdateHighlighting()
        {
            // Сбрасывается предыдущее выделение всех элементов
            foreach (var item in ArrayItems)
            {
                item.IsCurrentElement = false;
                item.IsModified = false;
            }

            // Выделяется текущий элемент массива, если индекс находится в допустимых пределах
            if (_isArrayCreated && _array != null && CurrentIndex >= 0 && CurrentIndex < _array.Length)
            {
                SelectedArrayIndex = CurrentIndex;
                if (CurrentIndex < ArrayItems.Count)
                {
                    ArrayItems[CurrentIndex].IsCurrentElement = true;
                }
            }
            else
            {
                SelectedArrayIndex = -1;
            }

            // Устанавливается цвет фона для переменных состояния
            CurrentIndexBackground = Brushes.LightYellow;
            CurrentResultBackground = Brushes.LightCyan;

            OnPropertyChanged(nameof(CurrentIndexBackground));
            OnPropertyChanged(nameof(CurrentResultBackground));
        }

        /// <summary>
        /// Обновление параметров в зависимости от выбранного режима работы
        /// </summary>
        private void UpdateMode()
        {
            if (IsPrefixSumMode)
            {
                InvariantDescription = "res равно сумме элементов a[0] до a[j-1]";
                InvariantFormula = "res = Σ_{i=0}^{j-1} a[i] ∧ 0 ≤ j ≤ n";
                VariantFunction = "t = n - j";
            }
            else if (IsPrefixMaxMode)
            {
                InvariantDescription = "res равно максимальному элементу среди a[0] до a[j-1]";
                InvariantFormula = "res = max(a[0..j-1]) ∧ 0 ≤ j ≤ n";
                VariantFunction = "t = n - j";
            }

            OnPropertyChanged(nameof(InvariantDescription));
            OnPropertyChanged(nameof(InvariantFormula));
            OnPropertyChanged(nameof(VariantFunction));
            OnPropertyChanged(nameof(WPRussianDescription));

            ResetExecution();
        }

        /// <summary>
        /// Сброс состояния выполнения к начальным значениям
        /// </summary>
        private void ResetExecution()
        {
            CurrentIndex = 0;
            SelectedArrayIndex = -1;

            if (_isArrayCreated && _array != null)
            {
                // Устанавливается начальное значение результата в зависимости от режима
                if (IsPrefixSumMode)
                    CurrentResult = 0;
                else if (IsPrefixMaxMode)
                    CurrentResult = (_array.Length > 0) ? int.MinValue : 0;

                // Выполняется проверка инварианта после инициализации
                Debug.Assert(CheckInvariantLogic(), "Инвариант нарушен после инициализации");

                UpdateHighlighting();
                CheckInvariantAfterStep();
                StatusMessage = "Состояние сброшено";
            }
            else
            {
                StatusMessage = "Создайте массив для начала работы";
                PostCondition = "Массив не создан";
            }

            UpdateState();
            OnPropertyChanged(nameof(StatusMessage));
        }

        /// <summary>
        /// Выполнение одного шага алгоритма
        /// </summary>
        private void ExecuteStep()
        {
            if (!_isArrayCreated || _array == null || _array.Length == 0)
            {
                StatusMessage = "Сначала создайте массив";
                OnPropertyChanged(nameof(StatusMessage));
                return;
            }

            if (CurrentIndex >= _array.Length)
            {
                StatusMessage = "Цикл завершен";
                OnPropertyChanged(nameof(StatusMessage));
                return;
            }

            try
            {
                UpdateHighlighting();

                // Проверяется выполнение инварианта перед выполнением шага
                Debug.Assert(CheckInvariantLogic(), "Инвариант нарушен до шага");

                CheckInvariantBeforeStep();

                // Вычисляется значение вариант-функции до выполнения шага
                int oldVariant = _array.Length - CurrentIndex;
                Debug.WriteLine($"Вариант-функция до шага: t = {oldVariant}");

                int oldResult = CurrentResult;

                // Выполняется операция в зависимости от выбранного режима
                if (IsPrefixSumMode)
                {
                    CurrentResult += _array[CurrentIndex];
                    if (CurrentIndex < ArrayItems.Count)
                    {
                        ArrayItems[CurrentIndex].IsModified = true;
                    }
                }
                else if (IsPrefixMaxMode)
                {
                    int oldMax = CurrentResult;
                    CurrentResult = Math.Max(CurrentResult, _array[CurrentIndex]);
                    if (CurrentResult != oldMax && CurrentIndex < ArrayItems.Count)
                    {
                        ArrayItems[CurrentIndex].IsModified = true;
                    }
                }

                CurrentIndex++;

                // Вычисляется значение вариант-функции после выполнения шага
                int newVariant = _array.Length - CurrentIndex;
                Debug.WriteLine($"Вариант-функция после шага: t = {newVariant} (убывание: {oldVariant} -> {newVariant})");

                // Проверяется выполнение инварианта после выполнения шага
                Debug.Assert(CheckInvariantLogic(), "Инвариант нарушен после шага");

                // Визуально выделяется изменение результата
                if (CurrentResult != oldResult)
                {
                    CurrentResultBackground = Brushes.LightGreen;
                    _ = System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CurrentResultBackground = Brushes.LightCyan;
                            OnPropertyChanged(nameof(CurrentResultBackground));
                        });
                    });
                }

                UpdateHighlighting();
                CheckInvariantAfterStep();
                UpdateState();
                StatusMessage = $"Шаг выполнен. Индекс: {CurrentIndex}, Результат: {CurrentResult}";
                OnPropertyChanged(nameof(StatusMessage));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка выполнения шага: {ex.Message}";
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        /// <summary>
        /// Вычисление weakest precondition для текущего состояния
        /// </summary>
        private string CalculateWP()
        {
            if (!_isArrayCreated || _array == null || CurrentIndex >= _array.Length)
                return "Не проверено";

            try
            {
                WpTrace.Clear();

                // Вычисляем новые значения
                int newRes = IsPrefixSumMode ?
                    CurrentResult + _array[CurrentIndex] :
                    Math.Max(CurrentResult, _array[CurrentIndex]);
                int newJ = CurrentIndex + 1;

                Stack<string> assignments = new Stack<string>();

                if (IsPrefixSumMode)
                {
                    assignments.Push($"j := {newJ}");
                    assignments.Push($"res := {newRes}");
                }
                else if (IsPrefixMaxMode)
                {
                    assignments.Push($"j := {newJ}");
                    assignments.Push($"res := {newRes}");
                }

                // Используем неравенства вместо равенств
                string invariant = $"res >= {newRes} && res <= {newRes} && j >= {newJ} && j <= {newJ}";

                string wpResult = _wpCalculator.CalculateForSequence(assignments, invariant);

                bool isValid = EvaluateCondition(wpResult);

                WPResult = isValid ? "Истина" : "Ложь";
                WPResultColor = isValid ? Brushes.LightGreen : Brushes.LightCoral;

                return wpResult;
            }
            catch (Exception ex)
            {
                WPResult = "Ошибка";
                WPResultColor = Brushes.LightCoral;
                return $"Ошибка: {ex.Message}";
            }
        }

        /// <summary>
        /// Проверка логики инварианта для текущего состояния
        /// </summary>
        private bool CheckInvariantLogic()
        {
            if (!_isArrayCreated || _array == null)
                return false;

            try
            {
                if (CurrentIndex < 0 || CurrentIndex > _array.Length)
                    return false;

                if (IsPrefixSumMode)
                {
                    // Вычисляется сумма элементов от начала до текущего индекса
                    int expectedSum = 0;
                    for (int i = 0; i < CurrentIndex && i < _array.Length; i++)
                    {
                        expectedSum += _array[i];
                    }
                    return CurrentResult == expectedSum && CurrentIndex >= 0 && CurrentIndex <= _array.Length;
                }
                else if (IsPrefixMaxMode)
                {
                    if (CurrentIndex == 0)
                        return CurrentResult == int.MinValue;

                    if (CurrentIndex > _array.Length)
                        return false;

                    // Вычисляется максимальный элемент среди обработанных
                    int expectedMax = _array[0];
                    for (int i = 1; i < CurrentIndex && i < _array.Length; i++)
                    {
                        if (_array[i] > expectedMax)
                            expectedMax = _array[i];
                    }
                    return CurrentResult == expectedMax && CurrentIndex >= 0 && CurrentIndex <= _array.Length;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CheckInvariantLogic error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Проверка инварианта перед выполнением шага
        /// </summary>
        private void CheckInvariantBeforeStep()
        {
            try
            {
                bool invariantHolds = CheckInvariantLogic();
                InvariantBeforeColor = invariantHolds ? Brushes.Green : Brushes.Red;
                InvariantBeforeStatus = invariantHolds ? "Истина: инвариант выполняется" : "Ложь: нарушение инварианта";
            }
            catch (Exception ex)
            {
                InvariantBeforeColor = Brushes.Red;
                InvariantBeforeStatus = $"Ошибка: {ex.Message}";
            }

            OnPropertyChanged(nameof(InvariantBeforeColor));
            OnPropertyChanged(nameof(InvariantBeforeStatus));
        }

        /// <summary>
        /// Проверка инварианта после выполнения шага
        /// </summary>
        private void CheckInvariantAfterStep()
        {
            try
            {
                bool invariantHolds = CheckInvariantLogic();
                InvariantAfterColor = invariantHolds ? Brushes.Green : Brushes.Red;
                InvariantAfterStatus = invariantHolds ? "Истина: инвариант сохранился" : "Ложь: инвариант нарушен";
            }
            catch (Exception ex)
            {
                InvariantAfterColor = Brushes.Red;
                InvariantAfterStatus = $"Ошибка: {ex.Message}";
            }

            OnPropertyChanged(nameof(InvariantAfterColor));
            OnPropertyChanged(nameof(InvariantAfterStatus));
        }

        /// <summary>
        /// Автоматическое выполнение всех шагов алгоритма
        /// </summary>
        private async void ExecuteRun()
        {
            if (!_isArrayCreated || _array == null || _array.Length == 0)
            {
                StatusMessage = "Сначала создайте массив";
                OnPropertyChanged(nameof(StatusMessage));
                return;
            }

            try
            {
                while (CurrentIndex < _array.Length)
                {
                    ExecuteStep();
                    await System.Threading.Tasks.Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка выполнения: {ex.Message}";
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        /// <summary>
        /// Обновление состояния интерфейса и верификационных данных
        /// </summary>
        private void UpdateState()
        {
            if (!_isArrayCreated || _array == null)
            {
                VariantCurrent = 0;
                VariantMax = 1;
                VariantProgressText = "Создайте массив";
                PostCondition = "Массив не создан";
                WPResult = "Не проверено";
                WPResultColor = Brushes.Gray;
                WPFormula = "(Inv ∧ B) ⇒ wp(S, Inv)";
            }
            else
            {
                VariantMax = _array.Length;
                VariantCurrent = _array.Length - CurrentIndex;
                VariantProgressText = $"t = {VariantCurrent} (убывает)";

                if (CurrentIndex >= _array.Length)
                {
                    if (IsPrefixSumMode)
                        PostCondition = $"res = Σ(i=0..{_array.Length - 1}) a[i] = {CurrentResult}";
                    else if (IsPrefixMaxMode)
                        PostCondition = $"res = max(a[0..{_array.Length - 1}]) = {CurrentResult}";
                }
                else
                {
                    PostCondition = "Цикл еще не завершен";
                }

                if (IsPrefixSumMode)
                {
                    WPRussianDescription =
                        "Если инвариант (res равна сумме элементов a[0..j-1]) верен и j < n, " +
                        "то после выполнения шага инвариант останется верным.";
                }
                else if (IsPrefixMaxMode)
                {
                    WPRussianDescription =
                        "Если инвариант (res равен максимуму элементов a[0..j-1]) верен и j < n, " +
                        "то после выполнения шага инвариант останется верным.";
                }

                try
                {
                    WPFormula = CalculateWP();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"UpdateState WP Error: {ex}");
                    WPFormula = $"Ошибка: {ex.Message}";
                    WPResult = "Ошибка";
                    WPResultColor = Brushes.LightCoral;
                }
            }

            OnPropertyChanged(nameof(VariantCurrent));
            OnPropertyChanged(nameof(VariantMax));
            OnPropertyChanged(nameof(VariantProgressText));
            OnPropertyChanged(nameof(PostCondition));
            OnPropertyChanged(nameof(WPFormula));
            OnPropertyChanged(nameof(WPResult));
            OnPropertyChanged(nameof(WPResultColor));
            OnPropertyChanged(nameof(WPRussianDescription));
        }

        /// <summary>
        /// Событие изменения свойства для реализации INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool EvaluateCondition(string condition)
        {
            try
            {
                // Простейшая проверка: вычисляем выражение как таблицу
                var expr = condition
                    .Replace("&&", " and ")
                    .Replace("||", " or ");

                var table = new System.Data.DataTable();
                table.Columns.Add("res", typeof(int));
                table.Columns.Add("j", typeof(int));

                table.Rows.Add(CurrentResult, CurrentIndex);

                return (bool)table.Compute(expr, "");
            }
            catch
            {
                return false;
            }
        }

    }

    /// <summary>
    /// Класс для представления элемента массива в интерфейсе
    /// </summary>
    public class ArrayItem : INotifyPropertyChanged
    {
        private int _value;
        private bool _isCurrentElement;
        private bool _isModified;

        public int Index { get; set; }

        public int Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        public bool IsCurrentElement
        {
            get => _isCurrentElement;
            set { _isCurrentElement = value; OnPropertyChanged(nameof(IsCurrentElement)); }
        }

        public bool IsModified
        {
            get => _isModified;
            set { _isModified = value; OnPropertyChanged(nameof(IsModified)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Реализация команды для привязки в WPF
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}