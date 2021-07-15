using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris2
{
    public partial class Form1 : Form
    {
        Pen pen_gray;
        Rectangle rec;
        Size dim_patrat;
        Size dim_joc;
        Point start_point;
        Timer timer;
        int[,] pozitii;
        Random random;
        int index_piesa;
        int rotire_piesa;
        int counter;
        

        public class Patrat
        {
            public Size size;
            public Point location;
            public Color color;
            public bool deseneaza;
            public Patrat()
            {
                this.size = new Size(37, 37);
                this.location = new Point(182, -18);
                this.color = Color.Blue;
            }
            public Patrat(Size size2, Point location2, Color color2)
            {
                this.size = size2;
                this.location = location2;
                this.color = color2;
            }
        }

        public struct Piesa
        {
            public Patrat[] p;
        }

        Piesa[] piesa;
        Patrat patrat;
        public List<Patrat> patrate;
        int LinieDeEliminat;
        Point start_piesa1;
        Color randomColor;
        public bool blocheaza_piesa;
        public int Scor;
        public int Minute;
        public int Secunde;

        public Form1()
        {
            counter = 0;
            InitializeComponent();
            Scor = 0;
            Minute = 0;
            Secunde = 0;
            rotire_piesa = 0;
            random = new Random();
            this.KeyPreview = true;
            timer = new Timer();
            pen_gray = new Pen(Color.Gray);
            pen_gray.Width = 3;
            dim_patrat = new Size(40, 40);
            dim_joc = new Size(360, 640);
            start_point = new Point(20, 20);
            rec = new Rectangle(start_point, dim_joc);
            this.DoubleBuffered = true;
            patrat = new Patrat();
            patrate = new List<Patrat>();
            pozitii = new int[20, 20];
            for (int i = 0; i < 20; ++i)
                for (int j = 0; j < 20; ++j)
                    pozitii[i, j] = 0;
            piesa = new Piesa[5];
            start_piesa1 = new Point(182, -18 + 3 * dim_patrat.Height);
            randomColor = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            for(int i = 0; i < 5; ++i)
                piesa[i].p = new Patrat[4];
            InitializarePiese();

            index_piesa = random.Next(0, 4);

            timer.Interval = 1000;
            timer.Start();
            timer.Tick += Timer_Tick;

        }

        private void VerificareRotire(int index, int rotiri)
        {
            bool rotatie = true;
            for (int i = 0; i < 4; ++i)
            {
                if (Verif(piesa[index].p[i]) == false || piesa[index].p[i].location.Y >= dim_joc.Height)
                {
                    rotatie = false;
                    break;
                }
                if (piesa[index].p[i].location.X < start_point.X)
                {
                    rotatie = false;
                    break;
                }
                if (piesa[index].p[i].location.X >= dim_joc.Width)
                {
                    rotatie = false;
                    break;
                }
            }

            
            if (rotatie == false)
            {
                rotire_piesa--;
                Rotire_Piesa(index, rotiri-1);
            }

        }

        private void InitializarePiese()
        {
            //piesa 0
            /*
             * 0110
             * 0110
             * 0000
             * 0000
             *
            */
            piesa[0].p[0] = new Patrat(dim_patrat, start_piesa1, randomColor);
            piesa[0].p[1] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + dim_patrat.Height), randomColor);
            piesa[0].p[2] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y), randomColor);
            piesa[0].p[3] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y + dim_patrat.Height), randomColor);


            //piesa 1
            /*
             * 0010
             * 0010
             * 0010
             * 0010
             *
            */
            piesa[1].p[0] = new Patrat(dim_patrat, start_piesa1, randomColor);
            piesa[1].p[1] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + 1 * dim_patrat.Height), randomColor);
            piesa[1].p[2] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + 2 * dim_patrat.Height), randomColor);
            piesa[1].p[3] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + 3 * dim_patrat.Height), randomColor);

            //piesa 2
            /*
             * 0010
             * 0010
             * 0011
             * 0000
             *
            */
            piesa[2].p[0] = new Patrat(dim_patrat, start_piesa1, randomColor);
            piesa[2].p[1] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + 1 * dim_patrat.Height), randomColor);
            piesa[2].p[2] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + 2 * dim_patrat.Height), randomColor);
            piesa[2].p[3] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y + 2 * dim_patrat.Height), randomColor);

            //piesa 3
            /*
             * 0110
             * 0011
             * 0000
             * 0000
             *
            */
            piesa[3].p[0] = new Patrat(dim_patrat, start_piesa1, randomColor);
            piesa[3].p[1] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y), randomColor);
            piesa[3].p[2] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y + dim_patrat.Height), randomColor);
            piesa[3].p[3] = new Patrat(dim_patrat, new Point(start_piesa1.X + 2 * dim_patrat.Width, start_piesa1.Y + dim_patrat.Height), randomColor);

            //piesa 4
            /*
             * 0100
             * 1110
             * 0000
             * 0000
             *
            */
            piesa[4].p[0] = new Patrat(dim_patrat, start_piesa1, randomColor);
            piesa[4].p[1] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + dim_patrat.Height), randomColor);
            piesa[4].p[2] = new Patrat(dim_patrat, new Point(start_piesa1.X + dim_patrat.Width, start_piesa1.Y + dim_patrat.Height), randomColor);
            piesa[4].p[3] = new Patrat(dim_patrat, new Point(start_piesa1.X - dim_patrat.Width, start_piesa1.Y + dim_patrat.Height), randomColor);
        }

        private void Rotire_Piesa(int index, int rotiri)
        {
            switch(index)
            {
                case 0:
                    //piesa 0: nici o schimbare
                    break;
                case 1:
                    //piesa 1: 2 posibile rotiri
                    if(rotiri % 2 == 0)
                    {
                        /*
                             * 0000
                             * *111
                             * 0000
                             * 0000
                             *
                             */
                        piesa[1].p[1].location.X = piesa[1].p[0].location.X;
                        piesa[1].p[2].location.X = piesa[1].p[0].location.X;
                        piesa[1].p[3].location.X = piesa[1].p[0].location.X;

                        piesa[1].p[1].location.Y = piesa[1].p[0].location.Y + dim_patrat.Width;
                        piesa[1].p[2].location.Y = piesa[1].p[1].location.Y + dim_patrat.Width;
                        piesa[1].p[3].location.Y = piesa[1].p[2].location.Y + dim_patrat.Width;
                        VerificareRotire(index, rotiri);
                    }
                    else
                    {
                        /*
                        * 0*00
                        * 0100
                        * 0100
                        * 0100
                        */
                        piesa[1].p[1].location.Y = piesa[1].p[0].location.Y;
                        piesa[1].p[2].location.Y = piesa[1].p[0].location.Y;
                        piesa[1].p[3].location.Y = piesa[1].p[0].location.Y;

                        piesa[1].p[1].location.X = piesa[1].p[0].location.X + dim_patrat.Width;
                        piesa[1].p[2].location.X = piesa[1].p[1].location.X + dim_patrat.Width;
                        piesa[1].p[3].location.X = piesa[1].p[2].location.X + dim_patrat.Width;
                        VerificareRotire(index, rotiri);
                    }
                    break;
                case 2:
                    //piesa 2: 4 posibile rotiri:
                    switch(rotiri % 4)
                    {
                        case 0:
                            /*
                             * 0000000
                             * 00*0000
                             * 0010000
                             * 0011000
                             * 0000000
                             *
                            */
                            piesa[2].p[1].location.Y = piesa[2].p[0].location.Y + dim_patrat.Height;
                            piesa[2].p[2].location.Y = piesa[2].p[1].location.Y + dim_patrat.Height;
                            piesa[2].p[3].location.Y = piesa[2].p[2].location.Y;

                            piesa[2].p[1].location.X = piesa[2].p[0].location.X;
                            piesa[2].p[2].location.X = piesa[2].p[0].location.X;
                            piesa[2].p[3].location.X = piesa[2].p[0].location.X + dim_patrat.Width;

                            VerificareRotire(index, rotiri);

                            break;
                        case 1:
                            /*
                            * 000010
                            * 00*110
                            * 000000
                            * 000000
                            * 000000
                            *
                           */
                            piesa[2].p[1].location.Y = piesa[2].p[0].location.Y;
                            piesa[2].p[2].location.Y = piesa[2].p[1].location.Y;
                            piesa[2].p[3].location.Y = piesa[2].p[2].location.Y - dim_patrat.Height;

                            piesa[2].p[1].location.X = piesa[2].p[0].location.X + dim_patrat.Width;
                            piesa[2].p[2].location.X = piesa[2].p[1].location.X + dim_patrat.Width;
                            piesa[2].p[3].location.X = piesa[2].p[2].location.X;

                            VerificareRotire(index, rotiri);


                            break;
                        case 2:
                            /*
                            * 000000
                            * 011000
                            * 001000
                            * 00*000
                            *
                           */
                            piesa[2].p[1].location.Y = piesa[2].p[0].location.Y - dim_patrat.Height;
                            piesa[2].p[2].location.Y = piesa[2].p[1].location.Y - dim_patrat.Height;
                            piesa[2].p[3].location.Y = piesa[2].p[2].location.Y;

                            piesa[2].p[1].location.X = piesa[2].p[0].location.X;
                            piesa[2].p[2].location.X = piesa[2].p[1].location.X;
                            piesa[2].p[3].location.X = piesa[2].p[2].location.X - dim_patrat.Width;
                            VerificareRotire(index, rotiri);
                            break;
                        case 3:
                            /*
                           * 0000000
                           * 0000000
                           * 0000000
                           * 011*000
                           * 0100000
                           * 0000000
                           *
                          */
                            piesa[2].p[1].location.Y = piesa[2].p[0].location.Y;
                            piesa[2].p[2].location.Y = piesa[2].p[1].location.Y;
                            piesa[2].p[3].location.Y = piesa[2].p[2].location.Y + dim_patrat.Height;

                            piesa[2].p[1].location.X = piesa[2].p[0].location.X - dim_patrat.Width;
                            piesa[2].p[2].location.X = piesa[2].p[1].location.X - dim_patrat.Width;
                            piesa[2].p[3].location.X = piesa[2].p[2].location.X;
                            VerificareRotire(index, rotiri);
                            break;
                    }
                    break;
                case 3:
                    //piesa 3: 4 posibile rotiri:
                    
                    switch (rotiri % 4)
                    {

                        case 0:
                             /*
                             * 0000
                             * 0011
                             * 0*10
                             * 0000
                             * 0000
                             * 0000
                             *
                             */
                            piesa[3].p[1].location.Y = piesa[3].p[0].location.Y;
                            piesa[3].p[2].location.Y = piesa[3].p[0].location.Y - dim_patrat.Height;
                            piesa[3].p[3].location.Y = piesa[3].p[0].location.Y - dim_patrat.Height;

                            piesa[3].p[1].location.X = piesa[3].p[0].location.X + dim_patrat.Width;
                            piesa[3].p[2].location.X = piesa[3].p[1].location.X;
                            piesa[3].p[3].location.X = piesa[3].p[2].location.X + dim_patrat.Width;
                            VerificareRotire(index, rotiri);
                            break;
                        case 1:
                            /*
                             * 0010
                             * 0110
                             * 0*00
                             *
                             */
                            piesa[3].p[1].location.Y = piesa[3].p[0].location.Y - dim_patrat.Height;
                            piesa[3].p[2].location.Y = piesa[3].p[1].location.Y;
                            piesa[3].p[3].location.Y = piesa[3].p[1].location.Y - dim_patrat.Height;

                            piesa[3].p[1].location.X = piesa[3].p[0].location.X;
                            piesa[3].p[2].location.X = piesa[3].p[1].location.X + dim_patrat.Width;
                            piesa[3].p[3].location.X = piesa[3].p[2].location.X;
                            VerificareRotire(index, rotiri);
                            break;
                        case 2:
                            /*
                             * 00000
                             * 11000
                             * 01*00
                             *
                             */
                            piesa[3].p[1].location.Y = piesa[3].p[0].location.Y;
                            piesa[3].p[2].location.Y = piesa[3].p[1].location.Y - dim_patrat.Height;
                            piesa[3].p[3].location.Y = piesa[3].p[2].location.Y;

                            piesa[3].p[1].location.X = piesa[3].p[0].location.X - dim_patrat.Width;
                            piesa[3].p[2].location.X = piesa[3].p[1].location.X;
                            piesa[3].p[3].location.X = piesa[3].p[2].location.X - dim_patrat.Width;
                            VerificareRotire(index, rotiri);
                            break;
                        case 3:
                            /*
                             * 00*00
                             * 00110
                             * 00010
                             *
                             */
                            piesa[3].p[1].location.Y = piesa[3].p[0].location.Y + dim_patrat.Height;
                            piesa[3].p[2].location.Y = piesa[3].p[1].location.Y;
                            piesa[3].p[3].location.Y = piesa[3].p[2].location.Y + dim_patrat.Height;

                            piesa[3].p[1].location.X = piesa[3].p[0].location.X;
                            piesa[3].p[2].location.X = piesa[3].p[1].location.X + dim_patrat.Width;
                            piesa[3].p[3].location.X = piesa[3].p[2].location.X;
                            VerificareRotire(index, rotiri);
                            break;
                    }
                    break;
                case 4:
                    //piesa 4: 4 posibile rotiri:
                    switch(rotiri % 4)
                    {

                        case 0:
                            /*
                             * 0*00
                             * 1110
                             * 0000
                             * 0000
                             *
                             */
                            piesa[4].p[1].location.Y = piesa[4].p[0].location.Y + dim_patrat.Height;
                            piesa[4].p[2].location.Y = piesa[4].p[0].location.Y + dim_patrat.Height;
                            piesa[4].p[3].location.Y = piesa[4].p[0].location.Y + dim_patrat.Height;
                                                             
                            piesa[4].p[1].location.X = piesa[4].p[0].location.X;
                            piesa[4].p[2].location.X = piesa[4].p[0].location.X - dim_patrat.Width;
                            piesa[4].p[3].location.X = piesa[4].p[0].location.X + dim_patrat.Width;
                            VerificareRotire(index, rotiri);
                            break;
                        case 1:
                            /*
                             * 0010
                             * 0*10
                             * 0010
                             * 0000
                             *
                             */
                            piesa[4].p[1].location.Y = piesa[4].p[0].location.Y;
                            piesa[4].p[2].location.Y = piesa[4].p[0].location.Y - dim_patrat.Height;
                            piesa[4].p[3].location.Y = piesa[4].p[0].location.Y + dim_patrat.Height;

                            piesa[4].p[1].location.X = piesa[4].p[0].location.X + dim_patrat.Width;
                            piesa[4].p[2].location.X = piesa[4].p[1].location.X;
                            piesa[4].p[3].location.X = piesa[4].p[2].location.X;
                            VerificareRotire(index, rotiri);
                            break;
                        case 2:
                            /* 1110
                             * 0*00
                             * 0000
                             * 0000
                             *
                             */
                            piesa[4].p[1].location.Y = piesa[4].p[0].location.Y - dim_patrat.Height;
                            piesa[4].p[2].location.Y = piesa[4].p[0].location.Y - dim_patrat.Height;
                            piesa[4].p[3].location.Y = piesa[4].p[0].location.Y - dim_patrat.Height;

                            piesa[4].p[1].location.X = piesa[4].p[0].location.X;
                            piesa[4].p[2].location.X = piesa[4].p[0].location.X - dim_patrat.Width;
                            piesa[4].p[3].location.X = piesa[4].p[0].location.X + dim_patrat.Width;
                            VerificareRotire(index, rotiri);
                            break;
                        case 3:
                            /*
                             * 1000
                             * 1*00
                             * 1000
                             * 0000
                             *
                             */
                            piesa[4].p[1].location.Y = piesa[4].p[0].location.Y;
                            piesa[4].p[2].location.Y = piesa[4].p[0].location.Y - dim_patrat.Height;
                            piesa[4].p[3].location.Y = piesa[4].p[0].location.Y + dim_patrat.Height;

                            piesa[4].p[1].location.X = piesa[4].p[0].location.X - dim_patrat.Width;
                            piesa[4].p[2].location.X = piesa[4].p[1].location.X;
                            piesa[4].p[3].location.X = piesa[4].p[2].location.X;
                            VerificareRotire(index, rotiri);
                            break;
                    }
                    break;
                default: break;
            }
        }

        private void Verf_Game_Over()
        {
            for(int j = 0; j < 9; ++j)
            {
                if(pozitii[3, j] == 1)
                {
                    timer.Enabled = false;
                    MessageBox.Show("Scorul tau final este: " + Scor.ToString(), "Sfarsitul jocului!", MessageBoxButtons.OK);
                    //de creeat posibilitatea de a juca din nou
                    button1.Enabled = false;
                    this.KeyPreview = false;
                    break;
                }
            }
        }

        private bool Verif(Patrat ptr1)
        {
            if (patrate == null)
                return true;
            foreach (Patrat ptr in patrate)
            {
                if (ptr1.location.X == ptr.location.X && ptr1.location.Y == ptr.location.Y )
                    return false;
            }
            return true;
        }

        private bool VerifJos_S(Patrat ptr1)
        {
            foreach (Patrat ptr in patrate)
            {
                if (ptr1.location.X == ptr.location.X && ptr1.location.Y + dim_patrat.Height == ptr.location.Y)
                    return false;
            }
            return true;
        }

        private bool VerifLaterale_A(Patrat ptr1)
        {
            foreach (Patrat ptr in patrate)
            {
                if (ptr1.location.X-dim_patrat.Width == ptr.location.X && ptr1.location.Y == ptr.location.Y)
                    return false;
            }
            return true;
        }

        private bool VerifLaterale_D(Patrat ptr1)
        {
            foreach (Patrat ptr in patrate)
            {
                if (ptr1.location.X + dim_patrat.Width == ptr.location.X && ptr1.location.Y == ptr.location.Y)
                    return false;
            }
            return true;
        }

        private void VerifMatrice()
        {
            
            foreach (Patrat ptr in patrate)
            {
                pozitii[ptr.location.Y / 40, ptr.location.X / 40] = 1;
            }
            int cnt = 0;
            
            for (int i = 0; i < 16; ++i)
            {
                cnt = 0;
                for (int j = 0; j < 9; ++j)
                {
                    if (pozitii[i, j] == 1)
                        cnt++;

                }
                if(cnt == 9)
                {
                    Scor += 250;
                    for(int d = i; d > 0; --d)
                    {
                        for (int j = 0; j < 9; ++j)
                        {
                            pozitii[d, j] = pozitii[d - 1, j];
                        }
                    }
                    LinieDeEliminat = i * 40 + 22;
                    patrate.RemoveAll(Linie);
                    for (int j = 0; j < 9; ++j)
                    {
                        pozitii[i, j] = 0;
                    }
                    foreach (Patrat ptr in patrate)
                    {
                        if (ptr.location.Y < LinieDeEliminat)
                            ptr.location.Y += dim_patrat.Height;
                    }
                }
            }
        }

        private bool Linie(Patrat ptr)
        {
            if (LinieDeEliminat == ptr.location.Y)
                return true;
            return false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Secunde++;
            if(Secunde == 60)
            {
                Minute++;
                Secunde = 0;
            }
            //patrat.location.Y += dim_patrat.Height;
            for (int i = 0; i < 4; ++i)
                piesa[index_piesa].p[i].location.Y += dim_patrat.Height;
            blocheaza_piesa = false;
            for (int i = 0; i < 4; ++i)
            {
                if (piesa[index_piesa].p[i].location.Y >= dim_joc.Height || Verif(piesa[index_piesa].p[i]) == false)
                {
                    blocheaza_piesa = true;
                    randomColor = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                    break;
                }
            }
            if (blocheaza_piesa == true)
            {
                Scor += 20;
                rotire_piesa = 0;
                for (int i = 0; i < 4; ++i)
                {
                    piesa[index_piesa].p[i].location.Y -= dim_patrat.Height;
                    patrate.Add(new Patrat(piesa[index_piesa].p[i].size, piesa[index_piesa].p[i].location, piesa[index_piesa].p[i].color));
                    //piesa[index_piesa].p[i] = new Patrat(dim_patrat, new Point(start_piesa1.X, start_piesa1.Y + i * dim_patrat.Height), randomColor);
                    piesa[index_piesa].p[i].color = randomColor;
                }
                InitializarePiese();
                index_piesa = random.Next(0, 5);
                for (int i = 0; i < 4; ++i)
                {
                    piesa[index_piesa].p[i].color = randomColor;
                }
            }
            /*
            if (patrat.location.Y >= dim_joc.Height || Verif(patrat) == false)
            {
                patrat.location.Y -= dim_patrat.Height;
                patrate.Add(new Patrat(patrat.size, patrat.location, patrat.color));
                patrat = new Patrat();
                patrat.color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
                //patrat.location.Y = -18;
                /*
                 * AdaugaInMatrice(patrat) -> adauga patratul in matricea de patrate
                 * -->idee: creem o matrice de tip bool si memoram 0 sau 1 in functie de
                 *  existenta patratelor in matricea de patrate;
                 * 
                 * verificam liniile din matricea de 0 si 1 si eliminam daca e cazul toate
                 * elementele de pe o linie completa;
                 * 
                 * coboram elementele
                 * 

             }
            */

            this.Invalidate();
            lblTimp.Text = Minute.ToString() + ":" + Secunde.ToString();
            lblScor.Text = Scor.ToString();
            VerifMatrice();
            Verf_Game_Over();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            //Desenare patrate prezente:
            foreach (Patrat ptr in patrate)
            {
                e.Graphics.FillRectangle(new SolidBrush(ptr.color), new Rectangle(ptr.location, ptr.size));
                //MessageBox.Show(Text = ptr.x.ToString() + "\n");
            }
            ///
            //Desenare bloc:
            // SolidBrush brush = new SolidBrush(patrat.color);
            // e.Graphics.FillRectangle(brush, new Rectangle(patrat.location, patrat.size));
            SolidBrush brush = new SolidBrush(randomColor);
            for (int i = 0; i < 4; ++i)
            {
                e.Graphics.FillRectangle(brush, new Rectangle(piesa[index_piesa].p[i].location, piesa[index_piesa].p[i].size));
            }


            //Desen cadru joc:
            e.Graphics.DrawRectangle(pen_gray, rec);
            for (int i = start_point.X + dim_patrat.Width; i < dim_joc.Width; i += dim_patrat.Width)
                e.Graphics.DrawLine(pen_gray, i, start_point.X, i, dim_joc.Height + start_point.Y);
            for (int i = start_point.Y + dim_patrat.Height; i < dim_joc.Height; i += dim_patrat.Width)
                e.Graphics.DrawLine(pen_gray, start_point.Y, i, start_point.X + dim_joc.Width, i);
            //------------------------

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Verificam marginile
            bool mutare = true;
            if (e.KeyChar == 'd' || e.KeyChar == 'D')
            {
                mutare = true;
                for (int i = 0; i < 4; ++i)
                {
                    if (piesa[index_piesa].p[i].location.X < dim_joc.Width - dim_patrat.Width)
                    {
                        if (VerifLaterale_D(piesa[index_piesa].p[i]) == false)
                        {
                            mutare = false;
                            break;
                        }
                    }
                    else
                    {
                        mutare = false;
                        break;
                    }
                }
                if(mutare == true)
                {
                    for (int i = 0; i < 4; ++i)
                        piesa[index_piesa].p[i].location.X += dim_patrat.Width;
                    this.Invalidate();
                }
                /*
                if (patrat.location.X < dim_joc.Width - dim_patrat.Width)
                {
                    if (VerifLaterale_D(patrat) == true)
                    {
                        patrat.location.X += dim_patrat.Width;
                        this.Invalidate();
                    }
                }
                */
            }

            if (e.KeyChar == 'A' || e.KeyChar == 'a')
            {
                mutare = true;
                for (int i = 0; i < 4; ++i)
                {
                    if (piesa[index_piesa].p[i].location.X > start_point.X + dim_patrat.Width)
                    {
                        if (VerifLaterale_A(piesa[index_piesa].p[i]) == false)
                        {
                            mutare = false;
                            break;
                        }
                    }
                    else
                    {
                        mutare = false;
                        break;
                    }
                }
                if (mutare == true)
                {
                    for (int i = 0; i < 4; ++i)
                        piesa[index_piesa].p[i].location.X -= dim_patrat.Width;
                    this.Invalidate();
                }
                /*
                if (patrat.location.X > start_point.X + dim_patrat.Width)
                {
                    if(VerifLaterale_A(patrat) == true)
                    {
                        patrat.location.X -= dim_patrat.Width;
                        this.Invalidate();
                    }
                }
                */
            }
            if (e.KeyChar == 'S' || e.KeyChar == 's')
            {

                mutare = true;
                for (int i = 0; i < 4; ++i)
                {
                    if (piesa[index_piesa].p[i].location.Y < dim_joc.Height - dim_patrat.Height)
                    {
                        if (VerifJos_S(piesa[index_piesa].p[i]) == false)
                        {
                            mutare = false;
                            break;
                        }
                    }
                    else
                    {
                        mutare = false;
                        break;
                    }
                }
                if (mutare == true)
                {
                    for (int i = 0; i < 4; ++i)
                        piesa[index_piesa].p[i].location.Y += dim_patrat.Height;
                    this.Invalidate();
                }
                /*
                if (patrat.location.Y < dim_joc.Height - dim_patrat.Height )
                {
                    patrat.location.Y += dim_patrat.Height;
                    if(Verif(patrat) == false)
                    {
                        patrat.location.Y -= dim_patrat.Height;
                    }
                    this.Invalidate();
                }
                */

            }

            //rotirea piesei:
            if (e.KeyChar == 'j' || e.KeyChar == 'J')
            {
                    rotire_piesa++;
                    Rotire_Piesa(index_piesa, rotire_piesa);
                    this.Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
            this.KeyPreview = !this.KeyPreview;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; ++i)
                for (int j = 0; j < 20; ++j)
                    pozitii[i, j] = 0;
            patrate.Clear();
            Scor = 0;
            Secunde = 0;
            Minute = 0;
            rotire_piesa = 0;
            InitializarePiese();
            this.Invalidate();
            timer.Enabled = true;
            index_piesa = random.Next(0, 5);
            button1.Enabled = true;
            this.KeyPreview = true;
        }
    }
}
