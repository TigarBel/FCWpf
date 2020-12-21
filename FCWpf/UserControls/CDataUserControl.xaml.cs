using FCWpf.Logic;
using FCWpf.Logic.Contact;
using FCWpf.Logic.Face;
using FCWpf.Windows;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace FCWpf.UserControls
{
  /// <summary>
  /// Логика взаимодействия для CDataUserControl.xaml
  /// </summary>
  public partial class CDataUserControl : UserControl
  {
    private SQLiteConnection _sqlite = new SQLiteConnection("Data Source=DMPM.db");

    public CDataUserControl()
    {
      InitializeComponent();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Update();
    }

    /// <summary>
    /// Обновить таблицу данных
    /// </summary>
    private void Update()
    {
      switch (comboBox.SelectedIndex) {
        case 0: {//Контакты
            dataGrid.ItemsSource = SelectQuery("SELECT FC_CONTACT.ID AS \"Инд№\", FC_CONTACT.ID_FC AS \"Инд№ Лица\", " +
              "FC_LSTYPE_CONTACT.NAME AS \"Тип контакта\", FC_CONTACT.CONTACT AS \"Контакт\", FC_CONTACT.DESCRIPTION AS \"Пояснение\" " +
              "FROM FC_CONTACT INNER JOIN FC_LSTYPE_CONTACT ON FC_CONTACT.ID_TYPE = FC_LSTYPE_CONTACT.ID").DefaultView;
            break;
          }
        case 1: {//Типы контактов
            dataGrid.ItemsSource = SelectQuery("SELECT * FROM FC_LSTYPE_CONTACT").DefaultView;
            break;
          }
        case 2: {//Лица
            dataGrid.ItemsSource = SelectQuery("SELECT FC_FACE.ID AS \"Инд№\", FC_LSTYPE.NAME AS \"Тип Лица\", FC_FACE.POST AS \"Должность\", " +
              "FC_FACE.FAMILY AS \"Фамилия\", FC_FACE.NAME1 AS \"Имя\", FC_FACE.NAME2 AS \"Отчество\", FC_FACE.NAME AS \"Организация\" FROM FC_FACE " +
              "INNER JOIN FC_LSTYPE ON FC_FACE.ID_TYPE = FC_LSTYPE.ID").DefaultView;
            break;
          }
        case 3: {//Типы лиц
            dataGrid.ItemsSource = SelectQuery("SELECT * FROM FC_LSTYPE").DefaultView;
            break;
          }
      }
    }

    /// <summary>
    /// Выполнить SQL запрос и получить таблицу с данными
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <returns>Таблица данных</returns>
    private DataTable SelectQuery(string query)
    {
      SQLiteDataAdapter sQLiteDataAdapter;
      DataTable dataTable = new DataTable();

      try {
        SQLiteCommand command;
        _sqlite.Open();  //Initiate connection to the db
        command = _sqlite.CreateCommand();
        command.CommandText = query;  //set the passed query
        sQLiteDataAdapter = new SQLiteDataAdapter(command);
        sQLiteDataAdapter.Fill(dataTable); //fill the datasource
      } catch (SQLiteException ex) {
        MessageBox.Show(ex.Message);
      }
      _sqlite.Close();
      return dataTable;
    }

    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
      try {
        switch (comboBox.SelectedIndex) {
          case 2: {//Лица
              CCreateWindow window = new CCreateWindow(null);
              window.ShowDialog();
              if (window.Have) {
                CIndividual individual = window.FaceWithContactID.Individual;

                if (individual is CEntityIndividual entityIndividual) {
                  SelectQuery("INSERT INTO FC_FACE (ID_TYPE, POST, FAMILY, NAME1, NAME2, NAME) " +
                    $"VALUES({(int)ETypeFace.Entity}, \"{entityIndividual.Post}\", \"{entityIndividual.Family}\", " +
                    $"\"{entityIndividual.Name}\",  \"{entityIndividual.Patronymic}\", \"{entityIndividual.Organization}\");");
                } else {
                  SelectQuery("INSERT INTO FC_FACE (ID_TYPE, POST, FAMILY, NAME1, NAME2, NAME) " +
                    $"VALUES({(int)ETypeFace.Individual}, \"\", \"{individual.Family}\", \"{individual.Name}\", " +
                    $"\"{individual.Patronymic}\", \"\");");
                }

                DataTable dataTable = SelectQuery("select max(id) from FC_FACE;");
                DataRow item = dataTable.Rows[0];
                int iIndex = Convert.ToInt32(item[0].ToString());
                foreach (CContactId contactId in window.FaceWithContactID.CContactIds) {
                  SelectQuery($"INSERT INTO FC_CONTACT (ID_FC, ID_TYPE, CONTACT, DESCRIPTION) VALUES({iIndex}, " +
                    $"{(int)contactId.Contact.GetTypeContract()}, \"{contactId.Contact.GetContact()}\", \"{contactId.Contact.Description}\"); ");
                }
              }
              
              break;
            }
          default: {
              throw new Exception("В данной таблице нельзя добавить запись!");
            }
        }
        Update();

      }
      catch(Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonChange_Click(object sender, RoutedEventArgs e)
    {
      try {
        if (dataGrid.SelectedIndex == -1) {
          throw new Exception("Выберете строку!");
        }

        switch (comboBox.SelectedIndex) {
          case 0: {//Контакты
              var v = GetContact();
              CContactWindow window = new CContactWindow(GetContact());
              window.ShowDialog();
              if (window.Have) {
                DataRowView item = (DataRowView)dataGrid.SelectedItem;
                string id = item.Row.ItemArray[0].ToString();
                string idFace = item.Row.ItemArray[1].ToString();

                CContact pContact = window.Contact;
                string sContact = pContact.GetContact();
                string description = pContact.Description;

                switch (pContact.GetTypeContract()) {
                  case ETypeContract.Phone: {
                      SelectQuery($"UPDATE FC_CONTACT SET ID_FC = {idFace}, ID_TYPE = \"{(int)ETypeContract.Phone}\", " +
                        $"CONTACT = \"{sContact}\", DESCRIPTION = \"{description}\" WHERE ID = {id};");
                      break;
                    }
                  case ETypeContract.Address: {
                      SelectQuery($"UPDATE FC_CONTACT SET ID_FC = {idFace}, ID_TYPE = \"{(int)ETypeContract.Address}\", " +
                        $"CONTACT = \"{sContact}\", DESCRIPTION = \"{description}\" WHERE ID = {id};");
                      break;
                    }
                  case ETypeContract.Email: {
                      SelectQuery($"UPDATE FC_CONTACT SET ID_FC = {idFace}, ID_TYPE = \"{(int)ETypeContract.Email}\", " +
                        $"CONTACT = \"{sContact}\", DESCRIPTION = \"{description}\" WHERE ID = {id};");
                      break;
                    }
                }
              }

              break;
            }
          case 2: {//Лица
              CCreateWindow window = new CCreateWindow(GetFaceWithContact());
              CFaceWithContactID faceWithContact = GetFaceWithContact();
              window.ShowDialog();
              if (window.Have) {
                CIndividual individual = window.FaceWithContactID.Individual;
                DataRowView item = (DataRowView)dataGrid.SelectedItem;
                string id = item.Row.ItemArray[0].ToString();

                if (individual is CEntityIndividual entityIndividual) {
                  SelectQuery($"UPDATE FC_FACE SET ID_TYPE = {(int)ETypeFace.Entity}, POST = \"{entityIndividual.Post}\", " +
                    $"FAMILY = \"{entityIndividual.Family}\", NAME1 = \"{entityIndividual.Name}\", NAME2 = \"{entityIndividual.Patronymic}\"" +
                    $", NAME = \"{entityIndividual.Organization}\" WHERE ID = {id};");
                } else {
                  SelectQuery($"UPDATE FC_FACE SET ID_TYPE = {(int)ETypeFace.Individual}, POST = \"\", " +
                    $"FAMILY = \"{individual.Family}\", NAME1 = \"{individual.Name}\", NAME2 = \"{individual.Patronymic}\"" +
                    $", NAME = \"\" WHERE ID = {id};");
                }

                DataTable dataTable = SelectQuery("select max(id) from FC_FACE;");
                DataRow row = dataTable.Rows[0];
                int iCounter = 0;

                foreach (CContactId contactId in faceWithContact.CContactIds) {
                  bool flagOnDelete = true;
                  iCounter++;

                  foreach (CContactId windowContactId in window.FaceWithContactID.CContactIds) {
                    if (contactId.ID == windowContactId.ID) {
                      SelectQuery($"UPDATE FC_CONTACT SET ID_TYPE = \"{(int)windowContactId.Contact.GetTypeContract()}\", " +
                        $"CONTACT = \"{windowContactId.Contact.GetContact()}\", DESCRIPTION = \"{windowContactId.Contact.Description}\" " +
                        $"WHERE FC_CONTACT.ID = {windowContactId.ID} AND FC_CONTACT.ID_FC = {id};");//Изменение контактов
                      flagOnDelete = false;
                      break;
                    }
                  }

                  if (flagOnDelete) {
                    SelectQuery($"DELETE FROM FC_CONTACT WHERE FC_CONTACT.ID = {contactId.ID}");//Удаление контактов
                    iCounter--;
                  }
                }

                for (int iIndex = iCounter; iIndex < window.FaceWithContactID.CContactIds.Count; iIndex++) {
                  CContactId windowContactId = window.FaceWithContactID.CContactIds[iIndex];
                  SelectQuery($"INSERT INTO FC_CONTACT (ID_FC, ID_TYPE, CONTACT, DESCRIPTION) VALUES({id}, " +
                      $"{(int)windowContactId.Contact.GetTypeContract()}, \"{windowContactId.Contact.GetContact()}\", " +
                      $"\"{windowContactId.Contact.Description}\"); ");//Добавление контактов
                }
              }

              break;
            }
          default: {
              throw new Exception("В данной таблице нельзя изменить запись!");
            }
        }
        Update();

      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private CContact GetContact()//Только для таблицы контакты
    {
      DataTable dataTable = SelectQuery("SELECT * FROM FC_CONTACT");
      DataRow item = dataTable.Rows[dataGrid.SelectedIndex];

      string contact = item[3].ToString();
      string description = item[4].ToString();
      switch (Convert.ToInt32(item[2])) {
        case (int)ETypeContract.Phone: {
            return new CPhoneContact(contact, description);
          }
        case (int)ETypeContract.Address: {
            return new CAddressContact(contact, description);
          }
        case (int)ETypeContract.Email: {
            return new CEmailContact(contact, description);
          }
      }
      return new CPhoneContact(contact, description);
    }

    private CFaceWithContactID GetFaceWithContact()//Только для таблицы Лица
    {
      DataRowView item = (DataRowView)dataGrid.SelectedItem;
      string family = item.Row.ItemArray[3].ToString();
      string name = item.Row.ItemArray[4].ToString();
      string patronymic = item.Row.ItemArray[5].ToString();
      CIndividual individual = new CIndividual(family, name, patronymic);
      ETypeFace typeFace = ETypeFace.Individual;

      if(item.Row.ItemArray[1].ToString() == "Юридическое лицо") {
        string post = item.Row.ItemArray[2].ToString();
        string organization = item.Row.ItemArray[5].ToString();
        individual = new CEntityIndividual(family, name, patronymic, post, organization);
        typeFace = ETypeFace.Entity;
      }

      DataTable dataTable = SelectQuery("SELECT * FROM FC_CONTACT WHERE FC_CONTACT.ID_FC = " + item.Row.ItemArray[0].ToString());

      CFaceWithContactID faceWithContact = new CFaceWithContactID(individual, typeFace);
      foreach (DataRow row in dataTable.Rows) {
        switch (Convert.ToInt32(row[2].ToString())) {
          case (int)ETypeContract.Phone: {
              faceWithContact.CContactIds.Add(new CContactId(new CPhoneContact(row[3].ToString(), row[4].ToString()), row[0].ToString()));
              break;
            }
          case (int)ETypeContract.Address: {
              faceWithContact.CContactIds.Add(new CContactId(new CAddressContact(row[3].ToString(), row[4].ToString()), row[0].ToString()));
              break;
            }
          case (int)ETypeContract.Email: {
              faceWithContact.CContactIds.Add(new CContactId(new CEmailContact(row[3].ToString(), row[4].ToString()), row[0].ToString()));
              break;
            }
        }
      }
      return faceWithContact;
    }

      private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
      try {
        if (dataGrid.SelectedIndex == -1) {
          throw new Exception("Не выбрана строка!");
        }

        switch (comboBox.SelectedIndex) {
          /*case 0: {//Контакты
          MessageBoxResult dialogResult = MessageBox.Show("Вы действительно хотите удалить данную запись?", "Внимание", MessageBoxButton.OKCancel);
              if (dialogResult == System.Windows.MessageBoxResult.Cancel) {
                return;
              }

              DataRowView item = (DataRowView)dataGrid.SelectedItem;
              string id = item.Row.ItemArray[0].ToString();
              SelectQuery("DELETE FROM FC_CONTACT WHERE FC_CONTACT.ID = " + id);

              break;
            }*/
          case 2: {//Лица
              MessageBoxResult dialogResult = MessageBox.Show("Вы действительно хотите удалить данную запись? Также удаляться записи в таблице Контакты.", 
                "Внимание", MessageBoxButton.OKCancel);
              if (dialogResult == System.Windows.MessageBoxResult.Cancel) {
                return;
              }

              DataRowView item = (DataRowView)dataGrid.SelectedItem;
              string id = item.Row.ItemArray[0].ToString();
              SelectQuery("DELETE FROM FC_CONTACT WHERE FC_CONTACT.ID_FC = " + id);
              SelectQuery("DELETE FROM FC_FACE WHERE FC_FACE.ID = " + id);
              
              break;
            }
          default: {
              throw new Exception("Из данной таблицы нельзя удалить записи!");
            }
        }
        Update();

      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
  }
}