using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Demo.WebProject.Properties;

namespace Demo.WebProject.Models
{
  public class HomeModel
  {
    [Display(ResourceType = typeof(Texts), Name="HelloWorld")]
    public string Name { get{ return "I'm a house!";}}
  }
}