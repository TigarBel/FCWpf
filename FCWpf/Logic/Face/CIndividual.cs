using System;

namespace FCWpf.Logic.Face
{
  /// <summary>
  /// Класс физического лица
  /// </summary>
  public class CIndividual
  {
    /// <summary>
    /// Фамилия
    /// </summary>
    private string _family;
    /// <summary>
    /// Имя
    /// </summary>
    private string _name;
    /// <summary>
    /// Отчество
    /// </summary>
    private string _patronymic;
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Family
    {
      get
      {
        return _family;
      }
      set
      {
        /*Ограничения*/
        if (value == null) {
          throw new Exception("Поле фамилии должно быть заполненным!");
        }
        if (value == "") {
          throw new Exception("Поле фамилии должно быть заполненным!");
        }
        _family = value;
      }
    }
    /// <summary>
    /// Имя
    /// </summary>
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        /*Ограничения*/
        if (value == null) {
          throw new Exception("Поле имя должно быть заполненным!");
        }
        if (value == "") {
          throw new Exception("Поле имя должно быть заполненным!");
        }
        _name = value;
      }
    }
    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronymic
    {
      get
      {
        return _patronymic;
      }
      set
      {
        /*Ограничения*/
        _patronymic = value;
      }
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="sFamily">Фамилия</param>
    /// <param name="sName">Имя</param>
    /// <param name="sPatronymic">Отчество</param>
    public CIndividual(string sFamily, string sName, string sPatronymic)
    {
      Family = sFamily;
      Name = sName;
      Patronymic = sPatronymic;
    }
  }
}
