using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Подключаем пространство имён графиков

namespace prog1
{
    public partial class Form1 : Form
    {
        Components[] comp; // Создаём массив класса компоненты
        public Form1()
        {
            InitializeComponent();
        }
        // Загрузка формы. Устанавливаем настройки таблицы, а также настройки файлового менеджера
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;

            openFileDialog1.Filter = "Text files(*.txt)|*.txt"; // Фильтр для файлового менеджера 
            openFileDialog1.Title = "Выберите файл txt, содержащий травяной сбор";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            saveFileDialog1.Title = "Сохраните травяной сбор в txt";
        }
        // Ввод общего веса и кол-ва трав
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var text = textBox1.Text.Split(); // Считываем текст и разделяем
                Herbs.allPersent = 100.0; // Указываем общий процент трав
                Herbs.allWeight = double.Parse(text[0]); // Указываем общий вес
                Herbs.countHerbs = int.Parse(text[1]); // Количество трав
                comp = new Components[Herbs.countHerbs]; // Инициализируем массив объектов
                createTable(comp.Length); // Взываем метод создания таблицы
                // Выводим на 2 лабел сколько осталось трав и процент трав
                label2.Text = String.Format("Вам осталось ввести {0} трав.\n Процент недостающих трав = {1} %", Herbs.countHerbs,Herbs.allPersent);
            }
            catch(Exception bug)
            {
                MessageBox.Show(bug.Message);
            }
        }
        // Функция добавить траву
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var text = textBox2.Text.Split();
                if (Herbs.countHerbs > 1) // Если осталось добавить больше 1 травы
                { 
                string name = text[0]; // Название
                double persent = double.Parse(text[1]); // Процент травы
                if (Herbs.allPersent < persent) // Если общий оставшийся процент меньше процента этой травы
                {
                    MessageBox.Show("Слишком большой процент содержания травы");
                    return;
                }
                    comp[comp.Length - Herbs.countHerbs] = new Components(name, persent); // Инициализируем объект класса
                    AddRowToTable(comp.Length - Herbs.countHerbs); // Добавляем строку в таблицу
                    Herbs.countHerbs--; // Уменьшаем количество незаполненных трав
                    if(Herbs.countHerbs == 1) // Если количество стало =1
                        label3.Text = "Введите ТОЛЬКО название травы: ";
                    Herbs.allPersent -= persent; // Уменьшаем общий процент
                    // ВЫводим сколько осталось ввести
                    label2.Text = String.Format("Вам осталось ввести {0} трав.\n Процент недостающих трав = {1} %", Herbs.countHerbs, Herbs.allPersent);
                    textBox2.Text = ""; 
                }
                else if (Herbs.countHerbs > 0) // Если осталось ввести 1 траву
                {
                    string name = text[0];
                    comp[comp.Length - Herbs.countHerbs] = new Components(name, Herbs.allPersent); // То создаём объект с отсавшимся общим процентом
                    AddRowToTable(comp.Length - Herbs.countHerbs); // Добавляем в таблицу
                    Herbs.countHerbs--; // Уменьшаем количество недобавленных трав
                    Herbs.allPersent = 0.0; // Задаём общий процент = 0
                    // Выводим, сколько осталось ввести
                    label2.Text = String.Format("Вам осталось ввести {0} трав.\n Процент недостающих трав = {1} %", Herbs.countHerbs, Herbs.allPersent);
                    textBox2.Text = "";
                    Diagram(); // Строим диаграмму
                }
            }
            catch (Exception bug)
            {
                MessageBox.Show(bug.Message);
            }
        }
        /// <summary>
        /// Функция создания таблицы
        /// </summary>
        /// <param name="countRows"> количество строк </param>
        // Функция создания таблицы заданного размера
        public void createTable(int countRows)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.RowCount = countRows; // Кол-во строк
            dataGridView1.ColumnCount = 3; // Кол-во столбцов = размеру массива
            dataGridView1.RowHeadersVisible = false; // Индексы строк не видны
            dataGridView1.ColumnHeadersVisible = true; // Индексы столбцов видны
            dataGridView1.Height = dataGridView1.ColumnHeadersHeight+countRows * dataGridView1.Rows[0].Height; 
            dataGridView1.Columns[0].Name = "Название травы"; 
            dataGridView1.Columns[1].Name = "Вес";
            dataGridView1.Columns[2].Name = "Процент содержания";
        }
        /// <summary>
        /// Функция заполнения строки в таблице
        /// </summary>
        /// <param name="numbRow"> Номер строки </param>
        public void AddRowToTable(int numbRow)
        {
            dataGridView1.Rows[numbRow].Cells[0].Value = comp[numbRow].name;
            dataGridView1.Rows[numbRow].Cells[1].Value = String.Format("{0} г",comp[numbRow].weight);
            dataGridView1.Rows[numbRow].Cells[2].Value = String.Format("{0} %",comp[numbRow].persent);
        }
        // Загрузить из файла
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = ""; // Устанвливаем изначально пустое имя файла в открытом менеджере файлов
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем менеджер. Если отмена, то выходим
                return;
            string filename = openFileDialog1.FileName; // Строка, сохраняющая имя файла
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(filename); // Создаём объект 
                var text = sr.ReadLine().Split();
                Herbs.allWeight = double.Parse(text[0]);
                Herbs.countHerbs = int.Parse(text[1]);
                Herbs.allPersent = 100.0;
                comp = new Components[Herbs.countHerbs];
                createTable(comp.Length);
                for(int i=0;i<comp.Length;i++)
                {
                    text = sr.ReadLine().Split(); // Считываем строку
                    comp[i] = new Components(text[0], double.Parse(text[1]));
                    AddRowToTable(i);
                }
                sr.Close(); // Закрываем поток
                Diagram();
            }
            catch (Exception bug)
            {
                MessageBox.Show(bug.Message);
            }
        }
        // Сохранить в файл
        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) // Открываем файловый менеджер. Если отмена, то выходим
                return;
            // получаем имя созданного файла
            string filename = saveFileDialog1.FileName;
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                sw.WriteLine(String.Format("{0} {1}", Herbs.allWeight, comp.Length));
                for (int i = 0; i < comp.Length; i++)
                    sw.WriteLine(String.Format("{0} {1}", comp[i].name, comp[i].persent));
                sw.Close();
            }
            catch(Exception bug)
            {
                MessageBox.Show(bug.Message);
            }
        }
        /// <summary>
        /// Функция построения круговой диаграммы
        /// </summary>
        private void Diagram()// метод рисует круговую диаграмму
        {
            chart1.Series.Clear();
            // Форматировать диаграмму
            chart1.BackColor = Color.Gray;
            chart1.BackSecondaryColor = Color.WhiteSmoke;
            chart1.BackGradientStyle = GradientStyle.DiagonalRight;

            chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            chart1.BorderlineColor = Color.Gray;
            chart1.BorderSkin.SkinStyle = BorderSkinStyle.None;

            // Форматировать область диаграммы
            chart1.ChartAreas[0].BackColor = Color.Wheat;

            // Добавить и форматировать заголовок
            chart1.Titles.Clear();
            chart1.Titles.Add("Состав травяного сбора");
            chart1.Titles[0].Font = new Font("Utopia", 16);

            chart1.Series.Add(new Series("ColumnSeries") { ChartType = SeriesChartType.Pie });

            string[] xValues = new string[comp.Length];
            double[] yValues = new double[comp.Length];
            for (int i = 0; i < comp.Length; i++)
            {
                xValues[i] = comp[i].name;
                yValues[i] = comp[i].persent;
            }
            chart1.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;//true;
        }

    }
}
