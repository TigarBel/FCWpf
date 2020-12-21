using FCWpf.Logic.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWpf.Logic
{
  /// <summary>
  /// Класс контак-id
  /// </summary>
  public class CContactId
  {
    /// <summary>
    /// Контакт
    /// </summary>
    public CContact Contact;
    /// <summary>
    /// Id
    /// </summary>
    public string ID;
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="contact">Контак</param>
    /// <param name="id">Id</param>
    public CContactId(CContact contact, string id)
    {
      Contact = contact;
      ID = id;
    }
  }
}
