using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core {

  public class Controls {

    public static string  RenderFooter(){
      return @"<div class=""Pie"" id=""IdProgressLocation"">
               <span class=""P"">Ministerio de esto y esto</span></div>";
    }

    public static string RenderMenu() {
      return @"<div class=""MenuContainer"" id=""Menu1"">
                <ul class=""Menu"">
                  <li class=""off"">Usuarios</li>
                  <li class=""off"">Tipos de empresa</li>
                  <li class=""off"">Tractores</li>
                </ul>
               </div>"; 
    }

    public static string RenderHeader() {
      return @"<div class=""HeaderContainer"">
                 <div class=""Logo""></div>
                 <div class=""Titulo"">SIRENTRA 2.0
                   <br>
                   <span>Sistema Informático de Registro de Transporte de Animales</span>
                 </div>
               </div>";
    }

  }



}
