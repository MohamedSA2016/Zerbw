using Xamarin.Forms;
using Zerbow;
using Zerbow.Droid.CustomRenderers;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Maps;
using Android.Content;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace Zerbow.Droid.CustomRenderers
{                                      
    public class ExtMapRenderer  : MapRenderer, IOnMapReadyCallback
    {
        bool isDrawn;
        public ExtMapRenderer(Context context) :base(context)
        {

        }
        // We use a native google map for Android
        private GoogleMap _map;

        public void OnMap(GoogleMap googleMap)
        {
            _map = googleMap;

            if (_map != null)
                _map.MapClick += googleMap_MapClick;
        }

 

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement !=null)
            {
              
                var formsMap = (ExtMap)e.NewElement;
                Control.GetMapAsync(this);
            }



            

            
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals("VisibleRegion") & !isDrawn)
            {
                NativeMap.SetPadding(0, 0, 0, 900);

                NativeMap.MyLocationEnabled = true;
                NativeMap.UiSettings.ZoomControlsEnabled = false;
                NativeMap.UiSettings.CompassEnabled = false;
                NativeMap.UiSettings.MyLocationButtonEnabled = true;
                NativeMap.BuildingsEnabled = false;

                isDrawn = true;
            }
        }
        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }


}