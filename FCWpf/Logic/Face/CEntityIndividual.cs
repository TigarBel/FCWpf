using System;

namespace FCWpf.Logic.Face
{
  public class CEntityIndividual : CIndividual
  {
    /// <summary>
    /// Должность
    /// </summary>
    private string _post;
    /// <summary>
    /// Организация
    /// </summary>
    private string _organization;
    /// <summary>
    /// Должность
    /// </summary>
    public string Post
    {
      get
      {
        return _post;
      }
      set
      {
        /*Ограничения*/
        if (value == null) {
          throw new Exception("Поле должность должно быть заполненным!");
        }
        if (value == "") {
          throw new Exception("Поле должность должно быть заполненным!");
        }
        _post = value;
      }
    }
    /// <summary>
    /// Организация
    /// </summary>
    public string Organization
    {
      get
      {
        return _organization;
      }
      set
      {
        /*Ограничения*/
        if (value == null) {
          throw new Exception("Поле организация должно быть заполненным!");
        }
        if (value == "") {
          throw new Exception("Поле организация должно быть заполненным!");
        }
        _organization = value;
      }
    }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="sFamily">Фамилия</param>
    /// <param name="sName">Имя</param>
    /// <param name="sPatronymic">Отчество</param>
    /// <param name="sPost">Должность</param>
    /// <param name="sOrganization">Организвация</param>
    public CEntityIndividual(string sFamily, string sName, string sPatronymic, string sPost, string sOrganization) : base(sFamily, sName, sPatronymic)
    {
      Post = sPost;
      Organization = sOrganization;
    }
  }
}
