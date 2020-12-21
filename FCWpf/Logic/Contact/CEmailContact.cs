namespace FCWpf.Logic.Contact
{
  /// <summary>
  /// Класс електронного адреса
  /// </summary>
  public class CEmailContact : CContact
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
    /// <param name="sContact">Електронный адрес</param>
    /// <param name="sDescription">Пояснение</param>
    public CEmailContact(string sContact, string sDescription) : base(sContact, sDescription, ETypeContract.Email)
    {
      Contact = sContact;
    }
  }
}
