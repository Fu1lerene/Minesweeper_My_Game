using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper_v3
{
    public partial class Form1 : Form
    {
        const int width = 10;    //
        const int height = 70;   // расстояния между клетками
        const int distance = 24; //

        int formSizeX = 275; // Размер игрового поля (окна)
        int formSizeY = 360; //

        ButtonExtended[,] Allbuttons; // массив кнопок (игровые клетки)
        Button button1; // кнопка перезапуска
        TextBox textBox1; // количество бомб
        TextBox textBox2; // таймер

        int fieldSizeX = 10; // размер игрового поля по умолчанию (уровень сложности)
        int fieldSizeY = 10; //

        int countTimer = 0; // счетчик для таймера
        int mCountBombs = 10; // количество бомб по умолчанию
        int temptempCountBombs; 
        int tempCountBombs = 0;
        int countClick = 0; // количество кликов

        Random rng = new Random(); // генератор случайных чисел

        // изображения
        Image Imwin = Image.FromFile("Images\\Win.png");
        Image Imlose = Image.FromFile("Images\\Lose.png");
        Image Imsmile = Image.FromFile("Images\\Smile.png");
        Image Imflag = Image.FromFile("Images\\Flag.png");
        Image ImBomb = Image.FromFile("Images\\Bomb.png");

        // цвета
        Color kr = Color.FromName("Red");
        Color gr = Color.FromName("LightGray");
        Color backDarkColor = Color.FromArgb(82, 14, 12); 

        // стили кнопки (для визуального различия нажатой кнопки)
        FlatStyle fl = FlatStyle.Flat;
        FlatStyle St = FlatStyle.Standard;

        // расположения
        ContentAlignment topcen = ContentAlignment.TopCenter;
        HorizontalAlignment center = HorizontalAlignment.Center;

        // стиль границы для текстбоксов
        BorderStyle noneborder = BorderStyle.None; 

        // шрифты
        Font timesNewRoman = new Font("Times New Roman", 19);
        Font timesNewRoman12 = new Font("Times New Roman", 12);
        Font timesNewRoman13 = new Font("Times New Roman", 13);

        Form cGame = new Form(); // игровое поле (создание новой формы для разных уровней сложности)

        // заголовки для кастомного задания уровня
        Label labelMain = new Label(); // главный заголовок 
        Label labelWidth = new Label(); // ширина
        Label labelHeight = new Label(); // высота
        Label labelMines = new Label(); // количество мин
        Label labelWidthInfo = new Label(); // информация о максимальной ширине
        Label labelHeightInfo = new Label(); // информация о максимальной высоте

        // текстбоксы
        TextBox textboxWidth = new TextBox(); // указание ширины
        TextBox textboxHeight = new TextBox(); // казание высоты
        TextBox textboxMines = new TextBox(); // указание количества мин

        Button buttonOK = new Button(); // подтвердить
        Button buttonCancel = new Button(); // отмена

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создание интерфейса игрового поля
        /// </summary>
        void CreateInterface()
        {
            // создание кнопки-смайлика (кнопка перезапуска)
            button1 = new Button();
            button1.Location = new Point(formSizeX / 2 - 25, 25);
            button1.Size = new Size(40, 40);

            // текст боксы с количеством мин и таймер
            textBox1 = new TextBox();
            textBox2 = new TextBox();

            // их расположение
            textBox1.Location = new Point(10, 30);
            textBox2.Location = new Point(formSizeX - 85, 30);

            // размер шрифта
            textBox1.Font = new Font(textBox1.Font.Name, 20, textBox1.Font.Style);
            textBox2.Font = new Font(textBox2.Font.Name, 20, textBox2.Font.Style);
            textBox2.Font = new Font(textBox1.Font, FontStyle.Regular);

            // размеры
            textBox1.Size = new Size(60, 45);
            textBox2.Size = new Size(60, 45);

            // задний фон
            textBox1.BackColor = backDarkColor;
            textBox2.BackColor = backDarkColor;

            // цвет текста
            textBox1.ForeColor = kr;
            textBox2.ForeColor = kr;

            // расположение текста по центру
            textBox1.TextAlign = center;
            textBox2.TextAlign = center;

            // удаляем границы текстбокса
            textBox1.BorderStyle = noneborder;
            textBox2.BorderStyle = noneborder;

            // стильно шрифта
            textBox1.Font = timesNewRoman;
            textBox2.Font = timesNewRoman;
            textBox1.Font = new Font(textBox1.Font, FontStyle.Bold);
            textBox2.Font = new Font(textBox2.Font, FontStyle.Bold);

            // обработчик событь при нажатии на кнопку-смайлик
            button1.Click += new EventHandler(button1_Click);

            // отображение заданных текстбоксов и кнопки
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(textBox2);

            // останавливаем таймер
            timer1.Stop();

            // наносим на кнопку картинку улыбающегося смайлика
            button1.Image = Imsmile;

            // конвертация в строчку
            textBox2.Text = Convert.ToString(countTimer);
            textBox1.Text = Convert.ToString(mCountBombs);
        }

        /// <summary>
        /// Старт новой игры
        /// </summary>
        void StartGame()
        {
            CreateInterface(); // создаем интерфейс

            temptempCountBombs = mCountBombs; // запоминаем количество бомб

            Allbuttons = new ButtonExtended[fieldSizeX, fieldSizeY]; // создаем игровые клетки количеством соответствующему заданному уровню сложности

            Size = new Size(formSizeX, formSizeY); // изменяем размер окна

            // создаем игровые клетки
            for (int x = width; (x - width) < fieldSizeX * distance; x += distance)
            {
                for (int y = height; (y - height) < fieldSizeY * distance; y += distance)
                {
                    ButtonExtended button = new ButtonExtended();
                    button.Location = new Point(x, y);
                    button.Size = new Size(25, 25);
                    button.BackColor = DefaultBackColor;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font(button.Font.Name, 12, button.Font.Style);
                    button.TextAlign = topcen;
                    Allbuttons[(x - width) / distance, (y - height) / distance] = button;
                    Controls.Add(button);
                    button.Click += new EventHandler(FieldClick);
                    button.MouseDown += new System.Windows.Forms.MouseEventHandler(button_MouseDown);
                }
            }
            CreateBombs(); // создаем бомбы
        }

        /// <summary>
        /// Созданием бомб
        /// </summary>
        void CreateBombs()
        {
            // цикл длится до тех пор, все бомбы не будут созданы
            while (tempCountBombs != mCountBombs)
            {
                // пробегаемся по всем клеткам по очереди
                for (int x = 0; x < fieldSizeX; ++x)
                {
                    for (int y = 0; y < fieldSizeY; ++y)
                    {
                        // дополнительная провекар воизбежание создания бОльшего количества бомб
                        if (tempCountBombs != mCountBombs)
                        {
                            if (rng.Next(0, 101) < 10 && !Allbuttons[x, y].isBomb) // создание бомбы с 10% шансом в каждой из клеток и проверка, что бомбы в данной клетки нет
                            {
                                Allbuttons[x, y].isBomb = true;
                                tempCountBombs++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Событие происходящее при клике на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FieldClick(object sender, EventArgs e)
        {
            ButtonExtended button = (ButtonExtended)sender;

            // проверка не является ли клетка помеченной флагом
            if (button.Image != Imflag)
            {
                timer1.Start(); // запускаем таймер

                // если клетка является бомбой
                if (button.isBomb)
                {
                    button.BackColor = kr; // окрашиваем задний фон в красный
                    Explode(button); // вызываем метод взрыв
                    button1.Image = Imlose; // меняем кнопку-смайлик на грустный

                }
                // если клетка не является бомбой
                else
                {
                    EmptyFieldClick(button); // вызываем метод для пустой клетки

                    // проверка на победу (если количество кликов по игровым клеткам будет равна размеру игровому поля - количество бомб)
                    if (countClick == (fieldSizeX*fieldSizeY-temptempCountBombs))
                    {
                        button1.Image = Imwin; // меняем кнопку-смайлик на победный
                        timer1.Stop(); // останавливаем таймер
                        mCountBombs = 0; // обнуляем счетчик бомб

                        // показываем все бомбы и где они находились
                        for (int x = 0; x < fieldSizeX; ++x)
                        {
                            for (int y = 0; y < fieldSizeY; ++y)
                            {
                                if (Allbuttons[x, y].isBomb)
                                {
                                    Allbuttons[x, y].Image = Imflag; // заменив картинки бомб на флаги
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Событие при нажатии на бомбу
        /// </summary>
        /// <param name="button"></param>
        void Explode(ButtonExtended button)
        {
            // показываем все бомбы и где они находились
            for (int x = 0; x < fieldSizeX; ++x)
            {
                for (int y = 0; y < fieldSizeY; ++y)
                {
                    if (Allbuttons[x, y].isBomb && Allbuttons[x, y].Image != Imflag)
                    {
                        Allbuttons[x, y].Image = ImBomb; // отображаем картинку бомбы
                    }
                }
            }
            GameOver(); // вызываем метод при окончании игры
            timer1.Stop(); // останавливаем таймер
        }

        /// <summary>
        /// Окончание игры
        /// </summary>
        void GameOver()
        {
            // запрещаем взаимодействовать с игровыми клетками после победы или поражения
            for (int x = 0; x < fieldSizeX; ++x)
            {
                for (int y = 0; y < fieldSizeY; ++y)
                {
                    if (!Allbuttons[x, y].isBomb)
                    {
                        Allbuttons[x, y].Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Событые при нажатии на пустую клетку
        /// </summary>
        /// <param name="button"></param>
        void EmptyFieldClick(ButtonExtended button)
        {
            // пробегаемся по всем клеткам
            for (int x = 0; x < fieldSizeX; ++x)
            {
                for (int y = 0; y < fieldSizeY; ++y)
                {
                    if (Allbuttons[x, y] == button)
                    {
                        if (Allbuttons[x, y].FlatStyle != fl) // если кнопка не была нажата ранее
                        {
                            countClick++; // увеличиваем количество кликов
                        }
                        else // если кнопка была нажата ранее
                        {
                            FastOpenClick(x, y); // вызываем метод открытия клеток в радиусе
                        }

                        Allbuttons[x, y].FlatStyle = fl; // изменяем вид на нажатый
                        Allbuttons[x, y].BackColor = gr; // окрашиваем задний фон
                        Allbuttons[x, y].Font = new Font(Allbuttons[x, y].Font, FontStyle.Bold); // изменяем шрифт

                        switch (CountBombsAround(x, y)) // указываем в клетки количество бомб вокруг и окрашиваем число в нужный цвет
                        {
                            case 0:
                                button.ForeColor = Color.Blue;
                                OpenEmptyField(x, y); // вызываем метод для открытия пустых клеток вокруг
                                break;
                            case 1:
                                button.ForeColor = Color.Blue;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 2:
                                button.ForeColor = Color.Green;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 3:
                                button.ForeColor = Color.Red;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 4:
                                button.ForeColor = Color.DarkBlue;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 5:
                                button.ForeColor = Color.Brown;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 6:
                                button.ForeColor = Color.Turquoise;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 7:
                                button.ForeColor = Color.Black;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                            case 8:
                                button.ForeColor = Color.Gray;
                                button.Text = "" + CountBombsAround(x, y);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Метод открытия клеток при нажатии на ранее открытую клетку (открытие всех клеток в радиусе)
        /// </summary>
        /// <param name="xF"></param>
        /// <param name="yF"></param>
        void FastOpenClick(int xF, int yF)
        {
            // пробегаемся по всем клеткам в радиусе
            for (int x = xF - 1; x <= xF + 1; ++x)
            {
                for (int y = yF - 1; y <= yF + 1; ++y)
                {
                    // проверка на ранее открытые клетки, клетки помеченные флагом и границы игрового поля
                    if (x >= 0 && x < fieldSizeX && y >= 0 && y < fieldSizeY && Allbuttons[x, y].FlatStyle != fl && Allbuttons[x, y].Image != Imflag)
                    {
                        switch (CountBombsAround(xF, yF)) // открываем необходимые клетки в зависимости от количества бомб вокруг
                        {
                            case 1:
                                if (CountFlagsAround(xF, yF) == 1)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 2:
                                if (CountFlagsAround(xF, yF) == 2)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 3:
                                if (CountFlagsAround(xF, yF) == 3)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 4:
                                if (CountFlagsAround(xF, yF) == 4)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 5:
                                if (CountFlagsAround(xF, yF) == 5)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 6:
                                if (CountFlagsAround(xF, yF) == 6)
                                    Allbuttons[x, y].PerformClick();
                                break;
                            case 7:
                                if (CountFlagsAround(xF, yF) == 7)
                                    Allbuttons[x, y].PerformClick();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Автоматическое открытие пустой клетки
        /// </summary>
        /// <param name="xO"></param>
        /// <param name="yO"></param>
        void OpenEmptyField(int xO, int yO)
        {
            for (int x = xO - 1; x <= xO + 1; ++x)
            {
                for (int y = yO - 1; y <= yO + 1; ++y)
                {
                    // проверка на границы поля и является ли клетка уже открытой
                    if (x >= 0 && x < fieldSizeX && y >= 0 && y < fieldSizeY && Allbuttons[x, y].FlatStyle != fl)
                    {
                        Allbuttons[x, y].PerformClick(); // открываем ее
                    }
                }
            }
        }

        /// <summary>
        /// Метод считающий количество флагов вокруг
        /// </summary>
        /// <param name="xB"></param>
        /// <param name="yB"></param>
        /// <returns></returns>
        int CountFlagsAround(int xB, int yB)
        {
            int countFlag = 0; // счетчик

            // пробегаемся по ближайшим клеткам
            for (int x = xB - 1; x <= xB + 1; ++x)
            {
                for (int y = yB - 1; y <= yB + 1; ++y)
                {
                    // провека на границы поля и является ли данная клетка флагом
                    if (x >= 0 && x < fieldSizeX && y >= 0 && y < fieldSizeY)
                        if (Allbuttons[x, y].Image == Imflag)
                        {
                            countFlag++; // прибавляем
                        }
                }
            }
            return countFlag;
        }

        /// <summary>
        /// Метод считающий количество бомб вокруг
        /// </summary>
        /// <param name="xB"></param>
        /// <param name="yB"></param>
        /// <returns></returns>
        int CountBombsAround(int xB, int yB)
        {
            int bombsCountAround = 0;// счетчик

            for (int x = xB - 1; x <= xB + 1; ++x)
            {
                for (int y = yB - 1; y <= yB + 1; ++y)
                {
                    // провека на границы поля и является ли данная клетка бомбой
                    if (x >= 0 && x < fieldSizeX && y >= 0 && y < fieldSizeY)
                        if (Allbuttons[x, y].isBomb)
                        {
                            bombsCountAround++; // прибавляем
                        }
                }
            }
            return bombsCountAround;
        }

        /// <summary>
        /// Перезапуск игры
        /// </summary>
        void RestartGame()
        {
            timer1.Stop(); // останавливаем таймер
            countTimer = 0; // счетчик таймера
            tempCountBombs = 0; // обнуляем количество бомб
            mCountBombs = temptempCountBombs; // записываем новое количество бомб

            textBox2.Text = Convert.ToString(countTimer);

            // для всех клеток возвращаем начальное состояние
            for (int x = 0; x < fieldSizeX; ++x)
            {
                for (int y = 0; y < fieldSizeY; ++y)
                {
                    Allbuttons[x, y].ResetText();
                    Allbuttons[x, y].Image = null;
                    Allbuttons[x, y].BackColor = DefaultBackColor;
                    Allbuttons[x, y].FlatStyle = St;
                    Allbuttons[x, y].isBomb = false;
                    Allbuttons[x, y].Enabled = true;
                }
            }

            button1.Image = Imsmile; // изменяем кнопку-смайлик на начальное состояние
            countClick = 0; // обнуляем счетчик кликов
            CreateBombs(); // создаем новые бомбы
        }

        /// <summary>
        /// Очистить поле
        /// </summary>
        void ClearField()
        {
            // удаляем текстбоксы и кнопку
            Controls.Remove(textBox2);
            Controls.Remove(textBox1);
            Controls.Remove(button1);

            // удаляем все игровые клетки
            for (int x = 0; x < fieldSizeX; ++x)
            {
                for (int y = 0; y < fieldSizeY; ++y)
                {
                    Controls.Remove(Allbuttons[x,y]);
                }
            }
        }
        
        /// <summary>
        /// Загрузка нового окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame();
            CreateInterfaceCustom();
        }

        /// <summary>
        /// Таймер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Invalidate(); // перерисовка таймербокса

            if (countTimer == 999) // остановка таймера при значении 999
            {
                timer1.Stop();
            }

            countTimer++;
            textBox2.Text = Convert.ToString(countTimer); // отображаем время
        }

        /// <summary>
        /// Таймер для обновления счетчика бомб
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox1.Invalidate(); // перерисовка таймербокса
            textBox1.Text = Convert.ToString(mCountBombs); // отображаем количество бомб
        }

        /// <summary>
        /// Событие при нажатии на кнопку-смайлик
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            RestartGame(); // перезапуск игры
        }

        /// <summary>
        /// Событие при нажатии на кнопку правой кнопкой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            // проверка на то, что игра продолжается
            if (e.Button == System.Windows.Forms.MouseButtons.Right && button1.Image != Imlose)
            {
                ButtonExtended button = (ButtonExtended)sender;

                if (button.Image != Imflag) // если клетка не ялвяется флагом
                {
                    if (button.FlatStyle != fl) // если клетка не открыта
                    {
                        button.Image = Imflag; // ставим флаг
                        mCountBombs--; // уменьшаем количество бомб для отображения в текстбоксе
                    }

                }
                else if (button1.Image != Imwin) // если клетка является флагом и игра не закончена (повторное нажатие на клетку с флагом)
                {
                    button.Image = null; // убираем флаг
                    mCountBombs++; // увеличиваем количество бомб для отображения в текстбоксе
                }
            }
        }

        /// <summary>
        /// Легкий уровень сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easy10x10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearField(); // очищаем поле

            // задаем необходимые параметры
            tempCountBombs = 0; 
            formSizeX = 275;
            formSizeY = 360;
            fieldSizeX = 10;
            fieldSizeY = 10;
            mCountBombs = 10;

            StartGame(); // начинаем игру
            RestartGame(); // перезапускаем во избежание проблем
        }

        /// <summary>
        /// Средний уровень сложности 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medium16x16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearField(); // очищаем поле

            // задаем необходимые параметры
            tempCountBombs = 0;
            formSizeX = 420;
            formSizeY = 500;
            fieldSizeX = 16;
            fieldSizeY = 16;
            mCountBombs = 40;

            StartGame(); // начинаем игру
            RestartGame(); // перезапускаем во избежание проблем
        }

        /// <summary>
        /// Сложный уровень сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hard30x16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearField(); // очищаем поле

            // задаем необходимые параметры
            tempCountBombs = 0;
            formSizeX = 755;
            formSizeY = 500;
            fieldSizeX = 30;
            fieldSizeY = 16;
            mCountBombs = 99;

            StartGame(); // начинаем игру
            RestartGame(); // перезапускаем во избежание проблем
        }

        /// <summary>
        /// Создание интерфейса для кастомного выбора сложности
        /// </summary>
        void CreateInterfaceCustom()
        {
            cGame.Size = new Size(350, 240); // задаем размер

            // расположение кнопок ок и отмена
            buttonOK.Location = new Point(80, 160);
            buttonCancel.Location = new Point(200, 160);

            buttonOK.Text = "OK";
            buttonCancel.Text = "Cancel";

            // шрифты
            textboxWidth.Font = timesNewRoman12;
            textboxHeight.Font = timesNewRoman12;
            textboxMines.Font = timesNewRoman12;

            // расположение текстбоксов
            textboxWidth.Location = new Point(70, 45);
            textboxHeight.Location = new Point(70, 80);
            textboxMines.Location = new Point(70, 115);

            // шрифты
            labelWidth.Font = timesNewRoman13;
            labelMines.Font = timesNewRoman13;
            labelHeight.Font = timesNewRoman13;
            labelMain.Font = timesNewRoman13;

            labelMain.Text = "Enter field size and number of mines";
            labelWidth.Text = "Width:";
            labelHeight.Text = "Height:";
            labelMines.Text = "Mines:";
            labelWidthInfo.Text = "Max: 63, Min: 10";
            labelHeightInfo.Text = "Max: 30";

            // размер главного заголовка
            labelMain.Size = new Size(300, 500);

            // расположение
            labelMain.Location = new Point(30, 10);
            labelWidth.Location = new Point(10, 47);
            labelHeight.Location = new Point(10, 82);
            labelMines.Location = new Point(10, 117);
            labelWidthInfo.Location = new Point(175, 50);
            labelHeightInfo.Location = new Point(175, 85);

            // отображение заголовков
            labelHeightInfo.Enabled = false;
            labelWidthInfo.Enabled = false;

            // обработчики событий при нажатии на кнопку
            buttonOK.Click += new EventHandler(ButtonOK);
            buttonCancel.Click += new EventHandler(ButtonCancel);

            // обработчики событий при вводе текста
            textboxWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textbox_KeyPress);
            textboxHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textbox_KeyPress);
            textboxMines.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textbox_KeyPress);
            textboxWidth.TextChanged += new System.EventHandler(textboxWidth_TextChanged);
            textboxHeight.TextChanged += new System.EventHandler(textboxHeight_TextChanged);
            textboxMines.TextChanged += new System.EventHandler(textboxMines_TextChanged);

            // отображение всех элементов
            cGame.Controls.Add(labelHeightInfo);
            cGame.Controls.Add(labelWidthInfo);
            cGame.Controls.Add(buttonOK);
            cGame.Controls.Add(buttonCancel);
            cGame.Controls.Add(textboxWidth);
            cGame.Controls.Add(textboxHeight);
            cGame.Controls.Add(textboxMines);
            cGame.Controls.Add(labelHeight);
            cGame.Controls.Add(labelWidth);
            cGame.Controls.Add(labelMines);
            cGame.Controls.Add(labelMain);
        }

        /// <summary>
        /// Кастомный уровень сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cGame.ShowDialog(); // отображение окна для задания необходимых параметров

            // очищаем текст, если ранее уже пользовались данным окном
            textboxMines.ResetText();
            textboxHeight.ResetText();
            textboxWidth.ResetText();
        }

        /// <summary>
        /// Проверка на допустимость входной ширины игрового поля при кастомном уровне сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxWidth_TextChanged(object sender, EventArgs e)
        {
            // максимальная ширина 63 клетки
            if (textboxWidth.Text != "" && Convert.ToInt32(textboxWidth.Text) > 63)
            {
                textboxWidth.Text = "63";
            }
            // автоматическое изменение высоты, если она меньше 10 клеток
            if (textboxHeight.Text != "" && Convert.ToInt32(textboxHeight.Text) < 10)
            {
                textboxHeight.Text = "10";
            }
        }

        /// <summary>
        /// Проверка на допустимость входной высоты игрового поля при кастомном уровне сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxHeight_TextChanged(object sender, EventArgs e)
        {
            // максимальная высота 30
            if (textboxHeight.Text != "" && Convert.ToInt32(textboxHeight.Text) > 30)
            {
                textboxHeight.Text = "30";
            }
            // автоматическое изменение ширины, если она меньше 10 клеток
            if (textboxWidth.Text != "" && Convert.ToInt32(textboxWidth.Text) < 10)
            {
                textboxWidth.Text = "10";
            }
        }

        /// <summary>
        /// Проверка на допустимость входных значений для количества бомб при кастомном уровне сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxMines_TextChanged(object sender, EventArgs e)
        {
             textboxMines.Enabled = true;
             int x = Convert.ToInt32(textboxWidth.Text);
             int y = Convert.ToInt32(textboxHeight.Text);

             // максимальное значение: высота*ширину
             if (textboxMines.Text != "" && Convert.ToInt32(textboxMines.Text) > x * y)
                {
                    textboxMines.Text = Convert.ToString(x * y);
                }
        }

        /// <summary>
        /// Проверка на корректность введенных значений в поля при кастомном уровне сложности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textboxWidth.Text != "" && textboxHeight.Text != "")
            {
                textboxMines.Enabled = true;
            }
            else textboxMines.Enabled = false;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        }

        /// <summary>
        /// Событие при нажатии кнопки ОК в кастомной игре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonOK(object sender, EventArgs e)
        {
            // проверка на корректность введеных значений
            if (textboxWidth.Text != "" && textboxHeight.Text != "" && textboxMines.Text != "")
            {
                ClearField(); // очищаем поле

                // устанавливаем необходимые параметры
                tempCountBombs = 0;
                int xOK = Convert.ToInt32(textboxWidth.Text);
                int yOK = Convert.ToInt32(textboxHeight.Text);
                int btsz = Allbuttons[0, 0].Width;
                formSizeX = xOK * distance + 35;
                formSizeY = yOK * distance + 115;
                fieldSizeX = Convert.ToInt32(textboxWidth.Text);
                fieldSizeY = Convert.ToInt32(textboxHeight.Text);
                mCountBombs = Convert.ToInt32(textboxMines.Text);

                cGame.Close(); // закрываем окно
                StartGame(); // начинаем игру
            }
        }

        /// <summary>
        /// Событие при нажатии кнопки Отмена в кастомной игре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ButtonCancel(object sender, EventArgs e)
        {
            Button buttonCancel = (Button)sender;
            cGame.Close(); // закрываем окно
        }

        /// <summary>
        /// Таймер для проверки корректности введеных мин в кастомной игре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (textboxWidth.Text != "" && textboxHeight.Text != "")
            {
                textboxMines.Enabled = true;
            }
            else textboxMines.Enabled = false;
        }
    }
    public class ButtonExtended : Button
    {
        public bool isBomb;
    }
}
