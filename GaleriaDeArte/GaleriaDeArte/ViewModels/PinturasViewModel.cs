using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using GaleriaDeArte.Models;
using GaleriaDeArte.Views;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json;

namespace GaleriaDeArte.ViewModels
{
    internal class PinturasViewModel : INotifyPropertyChanged
    {
        // Propiedades para manejar la lista de pinturas (galeria), la pintura en si, y
        // propiedades para manejar los errores y la ruta de los datos
        private ObservableCollection<Pintura> galeria;

        public ObservableCollection<Pintura> Galeria
        {
            get { return galeria; }
            set { galeria = value;
                PropertyChange();
            }
        }

        private Pintura pintura;

        public Pintura Pintura
        {
            get { return pintura; }
            set { pintura = value;
                PropertyChange();
            }
        }

        private string error;

        public string Error
        {
            get { return error; }
            set { error = value;
                PropertyChange();
            }
        }

        public string RutaDirectorio { get; set; }
        

        // Propiedades de comandos
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand MostrarDetallesCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand QuieresEliminarCommand { get; set; }

        // Variables
        int Posicion;


        // Vistas
        AgregarView agregarView;
        DetallesView DetallesView;
        EditarView editarView;

        // Constructor
        public PinturasViewModel()
        {
            Galeria = new ObservableCollection<Pintura>();
            RutaDirectorio = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "Galeria.json";
            CambiarVistaCommand = new Command<string>(CambiarVista);
            QuieresEliminarCommand = new Command<Pintura>(Mensaje);
            AgregarCommand = new Command(Agregar);
            MostrarDetallesCommand = new Command<Pintura>(MostrarDetalles);
            EditarCommand = new Command<Pintura>(Editar);
            GuardarCommand = new Command(Guardar);
            CargarDatos();
            PropertyChange();
            //Galeria.Add(new Pintura
            //{
            //    Nombre = "Girasoles",
            //    Artista = "Angel",
            //    Aaaa = 1995,
            //    Descripcion = "dsvf sfg efs  rer tbf ter sdc wrv sdbsdvfsdbf  sdfbd bte bd",
            //    Imagen = "https://media.admagazine.com/photos/61eb22cb9b19d943aa117b30/master/w_1600%2Cc_limit/Girasol.jpg"
            //});
        }

        public void Agregar()
        {
            if (Pintura != null)
            {
                if (String.IsNullOrWhiteSpace(Pintura.Nombre))
                {
                    Error = "Escriba el nombre de la obra.";
                }
                if (String.IsNullOrWhiteSpace(Pintura.Artista))
                {
                    Error = "Escriba el nombre del autor de la obra.";
                }
                if (String.IsNullOrWhiteSpace(Pintura.Aaaa.ToString()))
                {
                    Error = "Escriba el año de produccion de la obra.";
                }
                if (String.IsNullOrWhiteSpace(Pintura.Descripcion))
                {
                    Error = "Introdizca una breve descripcion de la obra.";
                }
                if (String.IsNullOrWhiteSpace(Pintura.Imagen))
                {
                    Error = "Introduzca un enlace valido para la imagen de la pintura.";
                }
                Galeria.Add(Pintura);
                CambiarVista("Ver");               
                GuardarDatos();
                PropertyChange();
            }
        }

        public void Guardar()
        {
            Galeria[Posicion] = Pintura;
            App.Current.MainPage.Navigation.PopToRootAsync();
            GuardarDatos();
        }

        public void CambiarVista(string vista)
        {
            if (vista == "Agregar")
            {
                Pintura = new Pintura();
                agregarView = new AgregarView()
                {
                    BindingContext = this
                };
                Application.Current.MainPage.Navigation.PushAsync(agregarView);
            }
            else if (vista == "Ver")

            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
        }

        public void MostrarDetalles(Pintura pintura)
        {
            DetallesView = new DetallesView();
            DetallesView.BindingContext = pintura;
            App.Current.MainPage.Navigation.PushAsync(DetallesView);
        }

        public void Editar(Pintura pintura)
        {
            if (pintura == null)
            {
                return;
            }
            Posicion = Galeria.IndexOf(pintura);
            Pintura = new Pintura
            {
                Nombre = pintura.Nombre,
                Artista = pintura.Artista,
                Aaaa = pintura.Aaaa,
                Descripcion = pintura.Descripcion,
                Imagen = pintura.Imagen

            };
            editarView = new EditarView
            {
                BindingContext = this
            };
            GuardarDatos();
            App.Current.MainPage.Navigation.PushAsync(editarView);
        }

        public void Eliminar()
        {
            Galeria.Remove(Pintura);
            GuardarDatos();
        }

        public async void Mensaje(Pintura pintura)
        {
            bool option = await Application.Current.MainPage.DisplayAlert("Eliminar", "¿Estas seguro de eliminar esta obra?", "Si", "No");
            if (option)
            {
                Pintura = pintura;
                Eliminar();
            }
        }











        // Persistencia de informacion
        public void GuardarDatos()
        {
            File.WriteAllText(RutaDirectorio, JsonConvert.SerializeObject(Galeria));
        }

        public void CargarDatos()
        {
            if (File.Exists(RutaDirectorio))
            {
                Galeria = JsonConvert
                    .DeserializeObject<ObservableCollection<Pintura>>
                        (File.ReadAllText(RutaDirectorio));
            }
            else
            {
                Galeria = new ObservableCollection<Pintura>();
            }
        }



        // Metodo para actualizar las propiedades


        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyChange(string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

    }
}
