using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notepad___
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool DateiGespeichert = true;
        string DateiName = "Unbenannt";
        SaveFileDialog s = new SaveFileDialog();
        OpenFileDialog o = new OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();
            SetTitle(DateiGespeichert, DateiName); 
            s.Filter = "Text Files (*.txt)|*.txt" + "|" +
                     "Image Files (*.png;*.jpg)|*.png;*.jpg" + "|" +
                     "All Files (*.*)|*.*";
            s.Title = "Save";
            
        }

        public void SetTitle(bool gesp, string name)
        {
            if(gesp && name == "")
            {
                Mein_Editor.Title =  "Unbenannt - Editor";
            }
            if(!gesp && name == "")
            {
                Mein_Editor.Title =  "*Unbenannt - Editor";
            }
            if(gesp && name != "")
            {
                Mein_Editor.Title = name + " - Editor";
            }
            if(!gesp && name != "")
            {
                Mein_Editor.Title = "*" + name + " - Editor";
            }
            //s.FileName = name;
            //s.ShowDialog(); 
            //if(s.ShowDialog() == DialogResult.OK)
            //{
  
            //}
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string header = item.Header.ToString();
            //Tbox.Text = header;
            switch (header)
            {
                case "Neu":
                    CreateNewFile();
                    break;
                case "Speichern":
                    Speichern();
                    break;
                case "Öffnen":
                    OeffnenFile();
                    break;
            };
      
        }

        private void Speichern()
        {
            if (DateiGespeichert)
            {
                //Tbox.Text = "";
            }
            else
            {
                MessageBoxResult erg = MessageBox.Show("Die Daten werden nicht gespeichert. Speichern?", "Achtung", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (erg.ToString())
                {
                    case "Yes":
                        //if(File.Exists(@"C:\Users\alex\Documents\sdfa.txt"))
                        if (File.Exists(DateiName))
                        {
                            File.WriteAllText(s.FileName, Tbox.Text);
                        }
                        else
                        {

                            if (s.ShowDialog() == true)
                            {
                                File.WriteAllText(s.FileName, Tbox.Text);
                            }
                            Tbox.Text = File.ReadAllText(o.FileName);
                            DateiName = o.FileName;
                            SetTitle(DateiGespeichert, DateiName);
                        }
                        DateiGespeichert = true;
                        SetTitle(DateiGespeichert, DateiName = s.FileName);
                        Tbox.Clear();
                        DateiGespeichert = true;

                        string [] pathArray = s.FileName.Split('\\');
                        DateiName = pathArray[pathArray.Length - 1];
            
                        SetTitle(DateiGespeichert, DateiName);
                        break;
                    case "Cansel":
                        break;
                }
            }
        }

        private void OeffnenFile()
        {
            if (o.ShowDialog() == true)
            {
                Tbox.Text = File.ReadAllText(o.FileName);
                DateiName = o.FileName;
                SetTitle(DateiGespeichert, DateiName);
            }
        }

        private void CreateNewFile()
        {

        }

        private void Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show("asdf");
            DateiGespeichert = false;
            SetTitle(DateiGespeichert, DateiName);
        }
    }
}
