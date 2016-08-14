using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ShadowList<T> : ObservableCollection<T>
    {
        private T[] ShadowArray = null;

        public ShadowList(ref T[] shadowArray)
        {
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(ShadowList_CollectionChanged);

            this.ShadowArray = shadowArray;
            if(this.ShadowArray.Length > 0)
            {
                foreach(T item in this.ShadowArray)
                {
                    Items.Add(item);
                }
            }
        }

        private void ShadowList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ShadowArray = this.ToArray();
        }

        /// <summary>  
        /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T).  
        /// </summary>  
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var i in collection) Items.Add(i);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>  
        /// Removes the first occurence of each item in the specified collection from ObservableCollection(Of T).  
        /// </summary>  
        public void RemoveRange(IEnumerable<T> collection) 
        { 
            foreach (var i in collection) Items.Remove(i); 
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); 
        } 
 
        /// <summary>  
        /// Clears the current collection and replaces it with the specified item.  
        /// </summary>  
        public void Replace(T item) 
        { 
            ReplaceRange(new T[] { item }); 
        } 

        /// <summary>  
        /// Clears the current collection and replaces it with the specified collection.  
        /// </summary>  
        public void ReplaceRange(IEnumerable<T> collection) 
        { 
            List<T> old = new List<T>(Items); 
            Items.Clear(); 
            foreach (var i in collection) Items.Add(i); 
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); 
        } 
 
    }
}
