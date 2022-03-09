using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dungeon_of_hell
{
    public interface IViewModel
    {
        public string Name { get; set; }
        public object ViewId { get; set; }
        public event Action<IViewModel,Type> addview;
        public event Action<string> removeview;
        public event Func<string, bool> viewexists;
        public event Action<string> changeprimaryviewEvent;
        public event Action<string> changesecondaryviewEvent;
        public event Action clearsecondviewEvent;
        public event Func<string, string, object> getviewpropertyEvent;
        public event Action<string, string,object> updateviewpropertyEvent;
        public abstract void KeyDown(object sender, KeyEventArgs e);
        /// <summary>
        /// Call event.
        /// </summary>
        /// <param name="viewname">View model's name.</param>
        public void AddView(IViewModel model, Type typeofview);
        public void RemoveView(string viewname);
        public bool ViewExists(string viewname);
        public void ChangePrimaryView(string viewname);
        /// <summary>
        /// Call event.
        /// </summary>
        /// <param name="viewname">View model's name.</param>
        public void ChangeSecondaryView(string viewname);
        /// <summary>
        /// Call event, it will set the view as null.
        /// </summary>
        public void ClearSecondView();
        /// <summary>
        /// Call event, it will return the property via reflection.
        /// </summary>
        /// <param name="viewname">View model's name.</param>
        ///<param name="property">The objects property.</param>
        public T GetViewProperty<T>(string viewname,string property);
        /// <summary>
        /// Call event, it will update the property via reflection.
        /// </summary>
        /// <param name="viewname">View model's name.</param>
        ///<param name="property">The objects property.</param>
        ///<param name="value">The objects new value.</param>
        public void UpdateViewProperty(string viewname, string property,object value);


    }
}
