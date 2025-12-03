using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notions.Core.Utils.Models.Fechas;

namespace Notions.Core.Utils.Common
{
    public class NgFechaNacimiento
    {
        public List<NgMesesModel> GetMeses()
        {
            List<NgMesesModel> Lista = new();
            NgMesesModel MesModel;

            MesModel = new NgMesesModel();
            MesModel.Id = 1;
            MesModel.Descripcion = "Enero";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 2;
            MesModel.Descripcion = "Febrero";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 3;
            MesModel.Descripcion = "Marzo";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 4;
            MesModel.Descripcion = "Abril";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 5;
            MesModel.Descripcion = "Mayo";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 6;
            MesModel.Descripcion = "Junio";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 7;
            MesModel.Descripcion = "Julio";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 8;
            MesModel.Descripcion = "Agosto";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 9;
            MesModel.Descripcion = "Septiembre";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 10;
            MesModel.Descripcion = "Octubre";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 11;
            MesModel.Descripcion = "Nobiembre";
            Lista.Add(MesModel);

            MesModel = new NgMesesModel();
            MesModel.Id = 12;
            MesModel.Descripcion = "Diciembre";
            Lista.Add(MesModel);

            return Lista;
        }
        public List<int> GetDias()
        {
            List<int> Lista = new List<int>();

            for (int i = 1; i <= 31; i++)
            {
                Lista.Add(i);
            }

            return Lista;
        }
        public List<int> GetAnios()
        {
            int desde = DateTime.Now.Year - 80;
            int hasta = DateTime.Now.Year - 15;
            List<int> Lista = new List<int>();

            for (int i = desde; i <= hasta; i++)
            {
                Lista.Add(i);
            }

            return Lista;
        }
    }
}
