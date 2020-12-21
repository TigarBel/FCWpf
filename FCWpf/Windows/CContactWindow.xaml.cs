using FCWpf.Logic.Contact;
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
  /// Логика взаимодействия для CContactWindow.xaml
  /// </summary>
  public partial class CContactWindow : Window
  {
    public bool Have = false;

    public CContact Contact
    {
      get
      {
        switch (comboBox.SelectedIndex) {
          case 0: {
              return new CPhoneContact(textBoxContact.Text, textBoxDescription.Text);
            }
          case 1: {
              return new CAddressContact(textBoxContact.Text, textBoxDescription.Text);
            }
          case 2: {
              return new CEmailContact(textBoxContact.Text, textBoxDescription.Text);
            }
          default: {
              throw new Exception("Данного вида контакта не существует!");
            }
        }
      }
      set
      {
        textBoxContact.Text = value.GetContact();
        textBoxDescription.Text = value.Description;
        comboBox.SelectedIndex = (int)value.GetTypeContract();
      }
    }

    public CContactWindow(CContact contact)
    {
      InitializeComponent();

      if (contact != null) {
        Contact = contact;
      }
    }

    private void ButtonAccept_Click(object sender, RoutedEventArgs e)
    {
      try {
        Have = true;
        this.Close();
      }
      catch (Exception ex) {
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
  }
}
