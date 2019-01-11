using Xamarin.Forms;
using Zerbow;
using Zerbow.Droid.CustomRenderers;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Maps;
using Android.Content;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace Zerbow.Droid.CustomRenderers
{                                      
    public class ExtMapRenderer  : MapRenderer, IOnMapReadyCallback
    {

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
            if(_map !=null)
            {
                _map.MapClick -= googleMap_MapClick;
            }


            base.OnElementChanged(e);

            if(Control !=null)
            {
                Control.GetMapAsync(this);
            }
        }

        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }


}