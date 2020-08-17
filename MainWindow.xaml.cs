using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
        bool umbruch = false;

        /// <summary>
        /// Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetTitle(DateiGespeichert, DateiName); 
            s.Filter = "Text Files (*.txt)|*.txt" + "|" + "Image Files (*.png;*.jpg)|*.png;*.jpg" + "|" + "All Files (*.*)|*.*";
            s.Title = "Save";
            
        }

        /// <summary>
        /// SetTitle changes the title of the main window depending on whether the file is saved or not and if its name is not empty 
        /// </summary>
        /// <param name="gesp"></param>
        /// <param name="name"></param>
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
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string header = item.Header.ToString();
            //Tbox.Text = header;
            switch (header)
            {
                case "Neu":
                    //CreateNewFile();
                    break;
                case "Speichern":
                    Speichern();
                    break;
                case "Öffnen":
                    OeffnenFile(e);
                    break;
                case "Speichern unter":
                    SpeichenrUnter();
                    break;
                case "Beenden":
                    Close();
                    break;
                case "Zeit":
                    break;
                case "Zeilenumbruch":
                    ZeilenUmbruchUmschalten();
                    break;
            };
      
        }

        #region Speichern
        private void Speichern()
        {
            if(File.Exists(DateiName) && !DateiGespeichert)
            {
                SpeichernOhneDialog();
            }
            else if(!File.Exists(DateiName) && !DateiGespeichert)
            {
                SpeichenrUnter();
            }
        }

        public void SpeichernOhneDialog()
        {
            File.WriteAllText(DateiName, Tbox.Text);
            DateiGespeichert = true;

            GetFilenameFromPath(DateiName);
            Tbox.Focus();
        }

        public void SpeichenrUnter ()
        {
            if (s.ShowDialog() == true)
            {
                File.WriteAllText(s.FileName, Tbox.Text);
                if (s.FileName != "")
                {
                    Tbox.Text = File.ReadAllText(s.FileName);
                    DateiGespeichert = true;
                    GetFilenameFromPath(s.FileName);
                }
            }
            Tbox.Focus();
        }
        #endregion

        /// <summary>
        /// calls open file dialog and opens the file
        /// </summary>
        private void OeffnenFile(RoutedEventArgs e)
        {

            bool open = true ;
            if (!DateiGespeichert)
            {

               open = DialogZeigen(e);
                
            }
            if (open)
            {
                if (o.ShowDialog() == true)
                {

                    System.IO.StreamReader sr = new StreamReader(o.FileName);
                    Tbox.Text = sr.ReadToEnd();
                    //Tbox.Text = File.ReadAllText(o.FileName);
                    DateiGespeichert = true;

                    DateiName = o.FileName;

                    GetFilenameFromPath(o.FileName);
                    sr.Close();
                    //Tbox.Undo();  
                }
            }
        }
        


        /// <summary>
        /// If text has been changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateiGespeichert = false;
            GetFilenameFromPath(DateiName);
        }

        /// <summary>
        /// Gets file name from the file path and calls SetTitle function
        /// </summary>
        /// <param name="filePath"></param>
        private void GetFilenameFromPath (string filePath)
        {
            string[] pathArray = filePath.Split('\\');

            string pureName = pathArray[pathArray.Length - 1];

            SetTitle(DateiGespeichert, pureName);
        }


        #region

        private bool DialogZeigen(RoutedEventArgs e)
        {

            bool open = true;
            if (!DateiGespeichert)
            {
                
                MessageBoxResult erg = MessageBox.Show("Die Daten werden nicht gespeichert. Speichern?", "Achtung", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (erg.ToString())
                {
                    case "Yes":
                        if (File.Exists(DateiName))
                        {
                            SpeichernOhneDialog();
                        }
                        else
                        {
                            SpeichenrUnter();
                        }

                        break;

                    case "No":
                        //open = false;
                        break;
                    case "Cancel":
                        open = false;
                        return open;
                }
            }
            return open;
        }
        private bool DialogZeigen(System.ComponentModel.CancelEventArgs e)
        {
            bool open = true;
            if (!DateiGespeichert)
            {
                MessageBoxResult erg = MessageBox.Show("Die Daten werden nicht gespeichert. Speichern?", "Achtung", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (erg.ToString())
                {
                    case "Yes":
                        if (File.Exists(DateiName))
                        {
                            SpeichernOhneDialog();
                        }
                        else
                        {
                            SpeichenrUnter();
                        }

                        break;

                    case "No":
                        //Close();
                        open = false;
                        break;
                    case "Cancel":
                        open = false;
                        //e.Cancel = true;
                        break;
                }
            }
            return open;
        }

        #endregion
        private void Mein_Editor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogZeigen(e);
        }


        private void ZeilenUmbruchUmschalten()
        {
             umbruch = !umbruch;
            if(umbruch)
            {
                Tbox.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                Tbox.TextWrapping = TextWrapping.NoWrap;
            }
        }
    }
}
