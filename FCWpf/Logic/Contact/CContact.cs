using System;

namespace FCWpf.Logic.Contact
{
  /// <summary>
  /// Базовый класс контакта
  /// </summary>
  public abstract class CContact
  {
    /// <summary>
    /// Контакт
    /// </summary>
    protected string _sContact;
    /// <summary>
    /// Пометка
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Тип контакта
    /// </summary>
    protected ETypeContract _typeContract;
    /// <summary>
    /// Конструктор базового класса
    /// </summary>
    /// <param name="sContact">Контак</param>
    /// <param name="sDescription">Пояснение</param>
    /// <param name="eTypeContract">Тип контакта</param>
    public CContact(string sContact, string sDescription, ETypeContract eTypeContract)
    {
      if(sContact == null || sDescription == null || eTypeContract == ETypeContract.Null) {
        throw new Exception("В одном из полей: контакты, пометка, тип контакта - пустое значение!");
      }
      if (sContact == "") {
        throw new Exception("В поле контакты пустое значение!");
      }
      _sContact = sContact;
      Description = sDescription;
      _typeContract = eTypeContract;
    }
    /// <summary>
    /// Получить контакт
    /// </summary>
    /// <returns>Контак</returns>
    public string GetContact()
    {
      return _sContact;
    }
    /// <summary>
    /// Получить тип контакта
    /// </summary>
    /// <returns>Тип контакта</returns>
    public ETypeContract GetTypeContract()
    {
      return _typeContract;
    }
  }
}
