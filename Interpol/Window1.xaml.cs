using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Interpol
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            MyInitialize();
        }
        public Window1(Users obj)
        {
            InitializeComponent();
            if (obj.Admin) { btnAddUsers.Visibility = Visibility.Visible; }
            else { btnAddUsers.Visibility = Visibility.Collapsed; }
            MyInitialize();
        }

        private void MyInitialize()
        {
            LoadDataBase();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)//выход из рабочего окна и возврат к окну авторизации
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        protected void LoadDataBase()//загрузка в датагрид данных с базы
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    var db = DbModel.Сriminals.ToList().Select(s => new GridCriminal(s));
                    BaseGrid.ItemsSource = null;
                    BaseGrid.ItemsSource = db.ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        protected void CleenCard()//
        {
            name.Text = string.Empty;
            lastname.Text = string.Empty;
            patronymic.Text = string.Empty;
            year.Text = string.Empty;
            status.Text = string.Empty;
            language.Text = string.Empty;
            Nacionaliti.Text = string.Empty;
            floor.SelectedIndex = -1;
            imgCriminal.Source = new BitmapImage(new Uri("VOR.png", UriKind.Relative));
            accusation.Text = string.Empty;
            txbUrlImg.Text = string.Empty;
            placebirth.Text = string.Empty;
        }
        private void BaseGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    if (BaseGrid.SelectedIndex < 0) return;
                    var ind = BaseGrid.SelectedIndex;
                    GridCriminal gc = BaseGrid.SelectedItem as GridCriminal;
                    if (gc == null) return;

                    var bc = DbModel.Сriminals.Where(b => b.Id == gc.Id).FirstOrDefault();
                    if (bc != null)
                    {
                        name.Text = gc.FirstName;
                        lastname.Text = gc.LastName;
                        patronymic.Text = gc.Patronymic;
                        year.Text = gc.Birthday;
                        status.Text = gc.Status;
                        language.Text = gc.Languages;
                        Nacionaliti.Text = gc.Nationality;
                        placebirth.Text = gc.PlaceOfBirth;
                        int i = -1;
                        foreach (ComboBoxItem item in floor.Items)
                        {
                            ++i;
                            if (gc.Gender.Contains(item.Content.ToString()))
                            {
                                floor.SelectedIndex = i;
                                break;
                            }
                        }

                        try { imgCriminal.Source = new BitmapImage(new Uri(bc.UrlFoto)); }
                        catch { }
                        
                        txbUrlImg.Text = bc.UrlFoto;
                        accusation.Text = bc.Сharges;
                        BaseGrid.SelectedIndex = ind;
                    }
                    else
                    {
                        imgCriminal.Source = null;
                        accusation.Text = "Нет данных!";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    if (BaseGrid.SelectedIndex < 0)
                    {
                        MessageBox.Show("Выберите строку данных!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    edit.IsEnabled = false;
                    save.IsEnabled = false;
                    delete.IsEnabled = false;

                    
                    GridCriminal gc = BaseGrid.SelectedItem as GridCriminal;
                    if (gc == null) return;

                    var bc = DbModel.Сriminals.Where(b => b.Id == gc.Id).FirstOrDefault();
                    if (bc != null)
                    {
                        MessageBoxResult Result = MessageBox.Show("Удалить данные? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Result == MessageBoxResult.No) { return; }
                        DbModel.Сriminals.Remove(bc);
                        DbModel.SaveChangesAsync().Wait();
                        CleenCard();
                        LoadDataBase();

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                edit.IsEnabled = true;
                save.IsEnabled = true;
                delete.IsEnabled = true;
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    if (BaseGrid.SelectedIndex < 0)
                    {
                        MessageBox.Show("Выберите строку данных!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    edit.IsEnabled = false;
                    save.IsEnabled = false;
                    delete.IsEnabled = false;
                    BaseGrid.IsEnabled = false;

                    var ind = BaseGrid.SelectedIndex;
                    GridCriminal gc = BaseGrid.SelectedItem as GridCriminal;
                    if (gc == null) return;

                    var bc = DbModel.Сriminals.Where(b => b.Id == gc.Id).FirstOrDefault();
                    if (bc != null)
                    {
                        MessageBoxResult Result = MessageBox.Show("Редактировать данные? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Result == MessageBoxResult.No) { return; }

                        bc.FirstName = name.Text;
                        bc.LastName = lastname.Text;
                        bc.Patronymic = patronymic.Text;
                        bc.Birthday = year.DisplayDate;
                        bc.Status = status.Text;
                        bc.Languages = language.Text;
                        bc.Nationality = Nacionaliti.Text;
                        bc.Gender = floor.Text;
                        bc.UrlFoto = txbUrlImg.Text;
                        bc.Сharges = accusation.Text;
                        bc.PlaceOfBirth = placebirth.Text;

                        DbModel.SaveChangesAsync().Wait();
                        CleenCard();
                        LoadDataBase();
                        BaseGrid.SelectedIndex = ind;

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                edit.IsEnabled = true;
                save.IsEnabled = true;
                delete.IsEnabled = true;
                BaseGrid.IsEnabled = true;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    edit.IsEnabled = false;
                    save.IsEnabled = false;
                    delete.IsEnabled = false;

                    if (!CheckFealds()){ return; }
                        
                    MessageBoxResult Result = MessageBox.Show("Записать данные? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);   
                    if (Result == MessageBoxResult.No) { return; }

                    Сriminals newCrim = new Сriminals();
                    newCrim.FirstName = name.Text;
                    newCrim.LastName = lastname.Text;
                    newCrim.Patronymic = patronymic.Text;
                    newCrim.Birthday = year.DisplayDate;
                    newCrim.Status = status.Text;
                    newCrim.Languages = language.Text;
                    newCrim.Nationality = Nacionaliti.Text;
                    newCrim.Gender = floor.Text;
                    newCrim.UrlFoto = txbUrlImg.Text;
                    newCrim.Сharges = accusation.Text;
                    newCrim.PlaceOfBirth = placebirth.Text;

                    DbModel.Сriminals.Add(newCrim);
                    DbModel.SaveChangesAsync().Wait();
                    CleenCard();
                    LoadDataBase();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally
            {
                edit.IsEnabled = true;
                save.IsEnabled = true;
                delete.IsEnabled = true;
            }
        }

        private void refesh_Click(object sender, RoutedEventArgs e)
        {
            CleenCard();
            LoadDataBase();
        }
        private bool CheckFealds()//проверка полей карточки на пустые значения
        {
            
            if (name.Text == string.Empty || lastname.Text == string.Empty ||
               patronymic.Text == string.Empty || year.Text == string.Empty ||
               status.Text == string.Empty || language.Text == string.Empty ||
               Nacionaliti.Text == string.Empty || floor.Text == string.Empty ||
               txbUrlImg.Text == string.Empty || accusation.Text == string.Empty)
            {
                MessageBox.Show("Не все поля заполнены!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            authorization auth = new authorization();
            auth.ShowDialog();
        }
        private void btbSearch_Click(object sender, RoutedEventArgs e)
        {

            LoadSearchCriminals(search.Text, BaseGrid);

        }
        protected void LoadSearchCriminals(string searchWord, DataGrid BaseGrid)//поиск в базе и загрузка в датагрид искомых данных
        {
            try
            {
                using (InterpolDataBaseEntities1 DbModel = new InterpolDataBaseEntities1())
                {
                    if (searchWord == string.Empty) return;
                    IEnumerable<GridCriminal> db = new List<GridCriminal>();
                    db = DbModel.Сriminals.ToList().Where(b => LongestCommonSubstring(b.FirstName, searchWord) ||
                    LongestCommonSubstring(b.LastName, searchWord) || LongestCommonSubstring(b.Patronymic, searchWord)).Select(s => new GridCriminal(s));
                    BaseGrid.ItemsSource = null;
                    BaseGrid.ItemsSource = db.ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public bool LongestCommonSubstring(string VariantWord, string SearchWord)//Алгоритм поиска наибольшей общей подстроки
        {
            VariantWord = VariantWord.ToUpper();
            SearchWord = SearchWord.ToUpper();
            var n = VariantWord.Length;
            var m = SearchWord.Length;
            var array = new int[n, m];
            var maxValue = 0;
            //var maxI = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (VariantWord[i] == SearchWord[j])
                    {
                        array[i, j] = (i == 0 || j == 0) ? 1 : array[i - 1, j - 1] + 1;
                        if (array[i, j] > maxValue)
                        {
                            maxValue = array[i, j];
                            //maxI = i;
                        }
                    }
                }
            }
            if (SearchWord.Length - maxValue <= (SearchWord.Length / 2))
            {
                return true;
            }
            else return false;
            //return VariantWord.Substring(maxI + 1 - maxValue, maxValue);
        }
    }
}
