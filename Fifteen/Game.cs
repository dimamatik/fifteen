using System;

namespace Fifteen
{
    /// <summary>
    /// Основной класс для игры на поле m*n 
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Поле с элементами
        /// </summary>
        private readonly int[] _field = null;

        /// <summary>
        /// Достать фишку, лежащую в заданных координатах
        /// </summary>
        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height) return 0;
                return _field[x + y * Width];
            }
        }
        /// <summary>
        /// Достать порядковый номер пустой фишки
        /// </summary>
        public int EmptyIndex()
        {
            return _index;
        }

        /// <summary>
        /// Размер поля, фактически Width * Height
        /// Одновременно - самый большой элемент
        /// </summary>
        private readonly int _max = 0;

        /// <summary>
        /// Индекс максимального элемента, для быстрого поиска
        /// </summary>
        private int _index = 0;

        /// <summary>
        /// Создать исходное прямоугольное поле с элементами 1...height*width
        /// </summary>
        /// <param name="height">Высота, больше 1</param>
        /// <param name="width">Ширина, больше 1</param>
        public Game (int width, int height)
        {
            if (height <= 1) throw new ArgumentOutOfRangeException("Высота равна " + height + ", но должна быть минимум 2");
            if (width <= 1) throw new ArgumentOutOfRangeException("Ширина равна " + width + ", но должна быть минимум 2");

            Height = height;
            Width = width;

            _max = Width * Height;
            _field = new int[_max];

            Reset();
        }

        /// <summary>
        /// Привести игровое поле в исходную позицию
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < _max; i++) _field[i] = i + 1;

            _index = _max - 1;
        }
        /// <summary>
        /// Привести игровое поле в произвольную разрешимую позицию
        /// </summary>
        public void Shuffle()
        {
            for (int i = 0; i < _max; i++) _field[i] = 0;
            
            var gen = new Random();
            for (int num = 1; num < _max; num++)
            {
                int rand = 1 + gen.Next(0, _max - num);
                for (int j = 0, k = 0; j < _max - 1; j++)
                {
                    if (_field[j] == 0) k++;
                    else continue;
                    if (k == rand) 
                    {
                        _field[j] = num;
                    }
                }
            }

            _field[_max - 1] = _max;
            _index = _max - 1;

            if (CheckSolvability() == false) _field.Swap(0, 1);

            int x = gen.Next(0, Width);
            int y = gen.Next(0, Height);


            Move(x, y);
        }

        /// <summary>
        /// Вычислить текущее количество инверсий в расстановке
        /// </summary>
        public int Number()
        {
            int n = 0;

            for (int i = 0; i < _max; i++)
            {
                int xi = _field[i];

                if (xi == _max) continue;

                for (int j = i + 1; j < _max; j++)
                {
                    if (_field[j] < xi) n++;
                }
            }
            return n;
        }
        /// <summary>
        /// Проверить, имеет ли текущая комбинация решение
        /// </summary>
        public bool CheckSolvability()
        {
            int n = Number();

            if (Width % 2 == 1) return n % 2 == 0;

            int row = 1 + _index / Width;

            return (n + row) % 2 == 0;
        }

        /// <summary>
        /// Передвинуть фишку в нужном направлении
        /// </summary>
        /// <returns>Возвращает TRUE, если удалось, FALSE иначе</returns>
        public bool Play(Direction dir)
        {
            int x = _index % Width;
            int y = _index / Width;

            switch (dir)
            {
                case Direction.LEFT:
                    if (x == 0) return false;
                    _field.Swap(_index, _index - 1);
                    _index = _index - 1;
                    return true;

                case Direction.RIGHT:
                    if (x == Width - 1) return false;
                    _field.Swap(_index, _index + 1);
                    _index = _index + 1;
                    return true;

                case Direction.UP:
                    if (y == 0) return false;
                    _field.Swap(_index, _index - Width);
                    _index = _index - Width;
                    return true;

                case Direction.DOWN:
                    if (y == Height - 1) return false;
                    _field.Swap(_index, _index + Width);
                    _index = _index + Width;
                    return true;

                default:
                    return false;
            }
        }
        /// <summary>
        /// Передвинуть фишку, если она рядом с пустой клеткой
        /// </summary>
        /// <returns>Возвращает TRUE, если удалось, FALSE иначе</returns>
        public bool Play(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height) return false;
            int x0 = _index % Width;
            int y0 = _index / Width;

            if (x0 == x)
            {
                if (y == y0 - 1) return Play(Direction.UP);
                else if (y == y0 + 1) return Play(Direction.DOWN);
                else return false;
            }
            else if (y0 == y)
            {
                if (x == x0 - 1) return Play(Direction.LEFT);
                else if (x == x0 + 1) return Play(Direction.RIGHT);
                else return false;
            }
            else return false;
        }
        /// <summary>
        /// Передвигает по змеевидному пути пустой элемент
        /// </summary>
        /// <returns>Возвращает TRUE, если удалось, FALSE иначе</returns>
        public bool Move(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height ||
                x == _index % Width && y == _index / Width) return false;

            while (x != _index % Width || y != _index / Width)
            {
                int _x = _index % Width;
                int _y = _index / Width;

                if (_y > y)
                {
                    if (_y % 2 == 0)
                    {
                        if (_x == 0) Play(Direction.UP);
                        else Play(Direction.LEFT);
                    }
                    else
                    {
                        if (_x == Width - 1) Play(Direction.UP);
                        else Play(Direction.RIGHT);
                    }
                }
                else if (_y < y)
                {
                    if (_y % 2 == 0)
                    {
                        if (_x == Width - 1) Play(Direction.DOWN);
                        else Play(Direction.RIGHT);
                    }
                    else
                    {
                        if (_x == 0) Play(Direction.DOWN);
                        else Play(Direction.LEFT);
                    }
                }
                else // _y == y
                {
                    if (_x < x) Play(Direction.RIGHT);
                    else Play(Direction.LEFT); // _x != x
                }
            }

            return true;
        }
        /// <summary>
        /// Проверить, что все фишки на местах
        /// </summary>
        public bool CheckVictory()
        {
            for (int i = 0; i < _max; i++)
            {
                if (_field[i] != i + 1) return false;
            }

            return true;
        }
    }
}
