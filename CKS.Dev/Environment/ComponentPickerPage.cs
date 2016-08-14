using System;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using System.Drawing;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    class ComponentPickerPage
        : UserControl
    {
        protected class ComponentPickerItem
        {
            public string Path { get; set; }
            public string Title { get; set; }
        }

        public ComponentPickerPage()
        {
            Padding = new Padding(12);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case VSConstants.CPPM_INITIALIZETAB:
                    HandleInitializeTabMessage(m);
                    break;
                case VSConstants.CPPM_INITIALIZELIST:
                    HandleInitializeItemsMessage(m);
                    break;
                case VSConstants.CPPM_QUERYCANSELECT:
                    HandleQueryCanSelectMessage(m);
                    break;
                case VSConstants.CPPM_SETMULTISELECT:
                    HandleSetMultiSelectMessage(m);
                    break;
                case VSConstants.CPPM_CLEARSELECTION:
                    HandleClearSelectionMessage(m);
                    break;
                case VSConstants.CPPM_GETSELECTION:
                    HandleGetSelectionMessage(m);
                    break;
                case Win32.WM_SIZE:
                    WmSize(m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected void NotifyItemDoubleClicked()
        {
            Win32.SendMessage(
                Win32.GetParent(Win32.GetParent(Win32.GetParent(Handle))),
                VSConstants.CPDN_SELDBLCLICK, 0, Handle.ToInt32());
        }

        protected void NotifySelectionChanged()
        {
            Win32.SendMessage(Win32.GetParent(Win32.GetParent(Win32.GetParent(Handle))),
                VSConstants.CPDN_SELCHANGED, 0, Handle.ToInt32());
        }

        protected virtual void InitializeItems() { }
        protected virtual bool GetCanSelect() { return true; }
        protected virtual void SetSelectionMode(bool multiSelect) { }
        protected virtual void ClearSelection() { }
        protected virtual ComponentPickerItem[] GetSelection() { return null; }

        void HandleInitializeTabMessage(Message m)
        {
            long exStyle = Win32.GetWindowLong(Win32.GetParent(Handle), Win32.GWL_EXSTYLE);
            if ((exStyle | Win32.WS_EX_CONTROLPARENT) != Win32.WS_EX_CONTROLPARENT)
            {
                exStyle ^= Win32.WS_EX_CONTROLPARENT;
                Win32.SetWindowLong(Win32.GetParent(Handle), Win32.GWL_EXSTYLE, exStyle);
            }
            Invalidate(true);
        }

        void HandleInitializeItemsMessage(Message m)
        {
            InitializeItems();
        }

        void HandleQueryCanSelectMessage(Message m)
        {
            Marshal.StructureToPtr(
                Convert.ToByte(GetCanSelect()),
                m.LParam,
                false);
        }
        void HandleSetMultiSelectMessage(Message m)
        {
            SetSelectionMode(Convert.ToBoolean((byte)m.LParam));
        }
        void HandleClearSelectionMessage(Message m)
        {
            ClearSelection();
        }

        void HandleGetSelectionMessage(Message m)
        {
            ComponentPickerItem[] items = GetSelection();
            int count = items != null ? items.Length : 0;
            Marshal.WriteInt32(m.WParam, count);
            if (count > 0)
            {
                IntPtr ppItems = Marshal.AllocCoTaskMem(
                  count * Marshal.SizeOf(typeof(IntPtr)));
                for (int i = 0; i < count; i++)
                {
                    IntPtr pItem = Marshal.AllocCoTaskMem(
                            Marshal.SizeOf(typeof(VSCOMPONENTSELECTORDATA)));
                    Marshal.WriteIntPtr(
                        ppItems + i * Marshal.SizeOf(typeof(IntPtr)),
                        pItem);
                    VSCOMPONENTSELECTORDATA data = new VSCOMPONENTSELECTORDATA()
                    {
                        dwSize = (uint)Marshal.SizeOf(typeof(VSCOMPONENTSELECTORDATA)),
                        bstrFile = items[i].Path,
                        bstrTitle = items[i].Title
                    };
                    Marshal.StructureToPtr(data, pItem, false);
                }
                Marshal.WriteIntPtr(m.LParam, ppItems);
            }
        }

        void WmSize(Message m)
        {
            if (m.WParam.ToInt32() == Win32.SIZE_RESTORED)
            {
                int newSize = m.LParam.ToInt32();
                short newWidth = (short)newSize;
                short newHeight = (short)(newSize >> 16);
                this.Size = new Size(newWidth, newHeight);
                Win32.SetWindowPos(Win32.GetParent(Handle),
                    IntPtr.Zero, 0, 0, newWidth, newHeight, 0);
                PerformLayout();
                Invalidate(true);
            }
        }
    }
}
