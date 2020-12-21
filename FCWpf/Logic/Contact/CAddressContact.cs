namespace FCWpf.Logic.Contact
{
  /// <summary>
  /// Класс городского адреса
  /// </summary>
  public class CAddressContact : CContact
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
        /*Ограничения*/
        _sContact = value;
      }
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="sContact">Адрес</param>
    /// <param name="sDescription">Пояснение</param>
    public CAddressContact(string sContact, string sDescription) : base(sContact, sDescription, ETypeContract.Address)
    {
      Contact = sContact;
    }
  }
}
