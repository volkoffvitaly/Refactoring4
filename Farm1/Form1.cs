using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farm1
{
    public partial class Form1 : Form
    {
        Dictionary<int, Animal> animal = new Dictionary<int, Animal>();
        Dictionary<int, Plant> plant = new Dictionary<int, Plant>();

        Random rnd = new Random();
        public int amountPlants = 30000;
        public int amountAnimals = 1000;
        public int diedPlants = 0;
        public int diedAnimals = 0;
        public int a = 0;
        public int date = 0;
        public int step = 0;
        public bool eat = false;

        public Form1()
        {
            InitializeComponent();

            for (int i = 1; i <= 1000000; i++)
            {
                plant.Add(i, new Plant());
            }
            for (int i = 1; i <= amountPlants; i++)
            {
                a = rnd.Next(1, 1000000);
                while (plant[a].existence)
                {
                    a = rnd.Next(1, 1000000);
                }
                plant[a].existence = true;
            }
            for (int i = 1; i <= amountAnimals; i++)
            {
                a = rnd.Next(1, 1000000);
                while (plant[a].existence)
                {
                    a = rnd.Next(1, 1000000);
                }
                animal.Add(i, new Animal());
                animal[i].x = a % 1000;
                animal[i].y = a / 1000;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(Pens.Black, 1, 101, 6001, 101);
            e.Graphics.DrawLine(Pens.Black, 1, 101, 1, 6101);
            for (int i = 1; i <= 1000; i++)
            {
                e.Graphics.DrawLine(Pens.Black, i * 6 + 1, 101, i * 6 + 1, 6101);
                e.Graphics.DrawLine(Pens.Black, 1, i * 6 + 101, 6001, i * 6 + 101);
            }

            for (int i = 1; i <= 1000000; i++)
            {
                if (plant[i].existence)
                {
                    e.Graphics.FillRectangle(Brushes.Green, (i % 1000) * 6 + 2, (i / 1000) * 6 + 102, 5, 5);
                }
            }

            for (int i = 1; i <= amountAnimals; i++)
            {
                if (animal[i].mind != 0)
                    e.Graphics.FillRectangle(Brushes.Red, animal[i].x * 6 + 2, animal[i].y * 6 + 102, 5, 5);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            date++;
            label1.Text = "Day: " + date;
            label2.Text = "Animals: " + (amountAnimals - diedAnimals);
            label4.Text = "Plants: " + (amountPlants - diedPlants);
            diedAnimals = 0;

            for (int i = 1; i <= amountAnimals; i++)
            {
                if (animal[i].mind != 0)
                {
                    step = animal[i].Step();
                    eat = plant[step].existence;
                    animal[i].Eat(eat);
                    plant[step].Die();
                    if (eat)
                        diedPlants++;
                }
                else
                    diedAnimals++;
            }

            if (diedPlants > 3000)
            {
                for (int i = amountPlants - diedPlants; i <= amountPlants; i++)
                {
                    a = rnd.Next(1, 1000000);
                    while (plant[a].existence)
                    {
                        a = rnd.Next(1, 1000000);
                    }
                    plant[a].existence = true;
                }
                diedPlants = 0;
            }

            if (date % 5 == 0)
                Refresh();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = 101 - (int)numericUpDown1.Value;
            timer1.Interval = r;
        }

    }

    class Plant
    {
        public int location = 0;
        public int x = 0;
        public int y = 0;
        public bool existence = false;

        public void Die()
        {
            existence = false;
        }
    }

    class Animal
    {
        public int satiety = 100;
        public int health = 100;
        public int location = 0;
        public int x = 0;
        public int y = 0;
        Random rnd = new Random();
        public int mind = 1;

        public int Step()
        {
            if (satiety > 0)
                satiety--;
            else
                health--;

            if (health == 0)
                Die();
            
            if (satiety > 40)
            {
                x += rnd.Next(-1, mind);
                y += rnd.Next(-1, mind);
            }
            else
            {
                x += rnd.Next(-1, mind);
                y += rnd.Next(-1, mind);
            }

            if (x < 0)
                x *= (-1);

            if (y < 0)
                y *= (-1);

            if (x > 1000)
                x = 1000 - x;

            if (y > 1000)
                y = 1000 - 1;


            location = x + y * 1000;

            if (location == 0)
                location++;

            return location;
        }

        public void Eat(bool success)
        {
            if (success)
            {
                satiety += 10;
            }
        }

        public void Die()
        {
            mind = 0;
        }
    }
}
