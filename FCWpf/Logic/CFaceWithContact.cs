using FCWpf.Logic.Contact;
using FCWpf.Logic.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWpf.Logic
{
  /// <summary>
  /// Класс лица с контактом-id
  /// </summary>
  public class CFaceWithContactID
  {
    /// <summary>
    /// Тип лица
    /// </summary>
    public ETypeFace TypeFace = ETypeFace.Individual;
    /// <summary>
    /// Лицо
    /// </summary>
    public CIndividual Individual;

    public List<CContactId> CContactIds = new List<CContactId>();
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="individual">Лицо</param>
    /// <param name="typeFace">Тип лица</param>
    public CFaceWithContactID(CIndividual individual, ETypeFace typeFace)
    {
      TypeFace = typeFace;
      Individual = individual;
    }
  }
}
