using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private List<Note> Notes { get; set; }
        private Note SelectedNote { get; set; }
        private DateTime SelectedDate { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SelectedDate = DateTime.Today;
            LoadNotes();
            UpdateNotesList();
        }

        private void LoadNotes()
        {
            string filePath = "notes.json"; // Путь к файлу с заметками
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Notes = JsonConvert.DeserializeObject<List<Note>>(json);
            }
            else
            {
                Notes = new List<Note>();
            }
        }

        private void UpdateNotesList()
        {
            List<Note> notesForSelectedDate = Notes.FindAll(n => n.Date.Date == SelectedDate.Date);
            NotesListBox.ItemsSource = notesForSelectedDate;
        }
        public class Note
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }

            public void SetTitle(string newTitle)
            {
                Title = newTitle;
            }

            public void SetDescription(string newDescription)
            {
                Description = newDescription;
            }
        }
        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = NewNoteTitleTextBox.Text,
                Description = NoteDescriptionTextBox.Text,
                Date = SelectedDate
            };


            Notes.Add(newNote);
            UpdateNotesList();
        }

        private void EditNote_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                // Предположим, что для обновления заметки используется метод с именем UpdateNote
                SelectedNote.Title = NewNoteTitleTextBox.Text;
                SelectedNote.Description = NoteDescriptionTextBox.Text;

                // Вызов метода для обновления списка заметок (например, если вы используете привязку данных)
                UpdateNotesList();
            }
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                Notes.Remove(SelectedNote);
                UpdateNotesList();
            }
        }
        public static void SerializeTasks(string filePath, ObservableCollection<Task> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(filePath, json);
        }

        public static ObservableCollection<Task> DeserializeTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<ObservableCollection<Task>>(json);
            }
            return new ObservableCollection<Task>();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "notes.json"); // Путь к файлу с заметками на рабочем столе
            string json = JsonConvert.SerializeObject(Notes);
            File.WriteAllText(filePath, json);
        }

        private void DatePicker_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            SelectedDate = DatePicker.SelectedDate ?? DateTime.Today;
            UpdateNotesList();
        }

        private void NotesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedNote = (Note)NotesListBox.SelectedItem;
            if (SelectedNote != null)
            {
                NewNoteTitleTextBox.Text = SelectedNote.Title;
                NoteDescriptionTextBox.Text = SelectedNote.Description;
            }
        }
    }
}
