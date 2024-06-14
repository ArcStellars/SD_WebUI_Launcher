using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Awake.Views.Pages;
using Awake.Models;
using System.Collections.ObjectModel;


namespace Awake.Services
{
    public static class Store
    {
        public static List<ExtRemote> extRemote;
        public static ObservableCollection<ExtItem> extLocal;
        public static ExtRemote extWebui;
    }
}
