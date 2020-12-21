using FCWpf.Logic;
using FCWpf.Logic.Contact;
using FCWpf.Logic.Face;
using FCWpf.UserControls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FCWpf.Windows
{
  /// <summary>
  /// Логика взаимодействия для CCreateWindow.xaml
  /// </summary>
  public partial class CCreateWindow : Window
  {
    
    private CFaceWithContactID _faceWithContactID;

    public CFaceWithContactID FaceWithContactID
    {
      get
      {
        return _faceWithContactID;
      }
      set
      {
        _faceWithContactID = value;

        textBoxFamily.Text = value.Individual.Family;
        textBoxName.Text = value.Individual.Name;
        textBoxPatronymic.Text = value.Individual.Name;

        if(value.Individual is CEntityIndividual entityIndividual) {
          comboBox.SelectedIndex = 1;
          textBoxPost.Text = entityIndividual.Post;
          textBoxOrganization.Text = entityIndividual.Organization;
        }

        foreach(CContactId contactId in value.CContactIds) {
          listBox.Items.Add(contactId.Contact.GetTypeContract().ToString() + ": " + contactId.Contact.GetContact());
        }
      }
    }

    public bool Have = false;

    public CCreateWindow(CFaceWithContactID faceWithContactID)
    {
      InitializeComponent();

      if (faceWithContactID != null) {
        FaceWithContactID = faceWithContactID;
      } else {
        _faceWithContactID = new CFaceWithContactID(new CIndividual("1", "1", "1"), ETypeFace.Individual);
      }

    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((ComboBox)sender).SelectedIndex == 0) {
        IsEnabledSelectionChanged(false);
      } else {
        IsEnabledSelectionChanged(true);
      }
    }

    /// <summary>
    /// Выставить пункты заполнения юр-лица
    /// </summary>
    /// <param name="flag">Истина/ложь</param>
    private void IsEnabledSelectionChanged(bool flag)
    {
      labelPost.IsEnabled = flag;
      labelOrganization.IsEnabled = flag;
      textBoxPost.IsEnabled = flag;
      textBoxOrganization.IsEnabled = flag;
    }
    

    private void ButtonAccept_Click(object sender, RoutedEventArgs e)
    {
      try {
        if(FaceWithContactID.CContactIds.Count == 0) {
          throw new Exception("Список контактов пуст!");
        }

        Have = true;

        switch (comboBox.SelectedIndex) {
          case (int)ETypeFace.Individual: {
              _faceWithContactID.Individual = new CIndividual(textBoxFamily.Text, textBoxName.Text, textBoxPatronymic.Text);
              _faceWithContactID.TypeFace = ETypeFace.Individual;
              break;
            }
          case (int)ETypeFace.Entity: {
              _faceWithContactID.Individual = new CEntityIndividual(textBoxFamily.Text, textBoxName.Text,
                textBoxPatronymic.Text, textBoxPost.Text, textBoxOrganization.Text);
              _faceWithContactID.TypeFace = ETypeFace.Entity;
              break;
            }
        }

        this.Close();
      }
      catch(Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      MessageBoxResult dialogResult = MessageBox.Show("Вы действительно хотите отменить действие?", "Внимание", MessageBoxButton.OKCancel);
      if (dialogResult == System.Windows.MessageBoxResult.OK) {
        this.Close();
      }
    }

    private void ButtonAddContact_Click(object sender, RoutedEventArgs e)
    {
      try {
        CContactWindow window = new CContactWindow(null);
        window.ShowDialog();

        if (window.Have) {
          _faceWithContactID.CContactIds.Add(new CContactId(window.Contact, "-1"));//Обозначаем, что в списке новая запись
          listBox.Items.Add(window.Contact.GetTypeContract().ToString() + ": " + window.Contact.GetContact());
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    private void ButtonRemoveContact_Click(object sender, RoutedEventArgs e)
    {
      if (listBox.SelectedIndex == -1) {
        return;
      }

      MessageBoxResult dialogResult = MessageBox.Show("Вы действительно хотите удалить выбранный контакт?", "Внимание", MessageBoxButton.OKCancel);
      if (dialogResult == System.Windows.MessageBoxResult.OK) {
        _faceWithContactID.CContactIds.RemoveAt(listBox.SelectedIndex);//Обозначаем, что убрали из списка
        listBox.Items.RemoveAt(listBox.SelectedIndex);
      }
    }

    private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      try {
        if (listBox.SelectedIndex == -1) {
          return;
        }

        CContactWindow window = new CContactWindow(_faceWithContactID.CContactIds[listBox.SelectedIndex].Contact);
        window.ShowDialog();

        if (window.Have) {
          _faceWithContactID.CContactIds[listBox.SelectedIndex].Contact = window.Contact;
          listBox.Items[listBox.SelectedIndex] = window.Contact.GetTypeContract().ToString() + ": " + window.Contact.GetContact();
        }
      }
      catch(Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
