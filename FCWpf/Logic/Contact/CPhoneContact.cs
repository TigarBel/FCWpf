using System;

namespace FCWpf.Logic.Contact
{
  /// <summary>
  /// Класс телефона
  /// </summary>
  public class CPhoneContact : CContact
  {
    /// <summary>
    /// Контакт
    /// </summary>
    public string Contact
    {
      get
      {
        return GetContact();
      }
      set
      {
        foreach(char cSymbol in value) {
          if(cSymbol < '0' || cSymbol > '9') {
            throw new Exception("Телефон содержит только цифры!");
          }
        }
        _sContact = value;
      }
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="sContact">Телефон</param>
    /// <param name="sDescription">Пояснение</param>
    public CPhoneContact(string sContact, string sDescription) : base(sContact, sDescription, ETypeContract.Phone)
    {
      Contact = sContact;
    }
  }
}
