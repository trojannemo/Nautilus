//===================================================================
//Copyright (C) 2010 Scott Wisniewski
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
//===================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    internal class OpenFileOrFolderDialog : CommonDialog
    {
        private int m_cancelWidth;
        private int m_selectWidth;
        private int m_buttonGap;
        private IntPtr m_hWnd;

        private bool m_useCurrentDir;
        private string m_currentFolder;
        private bool m_suppressSelectionChange;
        private bool m_hasDirChangeFired;

        private readonly InteropUtil.SUBCLASSPROC m_openFileSubClassDelegate;
        private readonly InteropUtil.WndProc m_hookDelegate;
        private readonly InteropUtil.SUBCLASSPROC m_defViewSubClassDelegate;

        private delegate void CalcPosDelegate(OpenFileOrFolderDialog @this, int baseRight, out int right, out int width);

        private static readonly Dictionary<int, CalcPosDelegate> m_calcPosMap =
            new Dictionary<int, CalcPosDelegate>
            {
                {
                    InteropUtil.ID_CUSTOM_CANCEL, 
                    (OpenFileOrFolderDialog @this, int baseRight, out int right, out int width) =>
                    {
                        right = baseRight;
                        width = @this.m_cancelWidth;
                    }
                },
                {
                    InteropUtil.ID_SELECT, 
                    (OpenFileOrFolderDialog @this, int baseRight, out int right, out int width) =>
                    {
                        right = baseRight - (@this.m_cancelWidth + @this.m_buttonGap);
                        width = @this.m_selectWidth;
                    }
                }
            };
        
        public string Path { get; set; }
        public string Title { get; set; }
        public bool ShowReadOnly { get; set; }
        public bool AcceptFiles { get; set; }
        public string FileNameLabel { get; set; }

        public OpenFileOrFolderDialog()
        {
            
            m_openFileSubClassDelegate = OpenFileSubClass;
            m_hookDelegate = HookProc;
            m_defViewSubClassDelegate = DefViewSubClass;
            Path = null;
            Title = null;
            m_useCurrentDir = false;
            AcceptFiles = true;
            m_currentFolder = null;
            m_useCurrentDir = false;
            m_cancelWidth = 0;
            m_selectWidth = 0;
            m_buttonGap = 0;
            m_hWnd = IntPtr.Zero;
        }

        public override void Reset()
        {
            Path = null;
            Title = null;
            m_useCurrentDir = false;
            AcceptFiles = true;
            m_currentFolder = null;
            m_useCurrentDir = false;
            m_cancelWidth = 0;
            m_selectWidth = 0;
            m_buttonGap = 0;
            m_hWnd = IntPtr.Zero;
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            Util.Assume(Marshal.SystemDefaultCharSize == 2, "The character size should be 2");
            
            var nativeBuffer = Marshal.AllocCoTaskMem(InteropUtil.NumberOfFileChars * 2);
            IntPtr filterBuffer = IntPtr.Zero;

            try
            {
                var openFileName = new InteropUtil.OpenFileName();
                openFileName.Initialize();
                openFileName.hwndOwner = hwndOwner;

                var chars = new char[InteropUtil.NumberOfFileChars];

                try
                {
                    if (File.Exists(Path))
                    {
                        if (AcceptFiles)
                        {
                            var fileName = System.IO.Path.GetFileName(Path);
                            var length = Math.Min(fileName.Length, InteropUtil.NumberOfFileChars);
                            fileName.CopyTo(0, chars, 0, length);
                            openFileName.lpstrInitialDir = System.IO.Path.GetDirectoryName(Path);
                        }
                        else
                        {
                            openFileName.lpstrInitialDir = System.IO.Path.GetDirectoryName(Path);
                        }
                    }
                    else if (Directory.Exists(Path))
                    {
                        openFileName.lpstrInitialDir = Path;
                    }
                    else
                    {
                        //the path does not exist.
                        //We don't just want to throw it away, however.
                        //The initial path we get is most likely provided by the user in some way.
                        //It could be what they typed into a text box before clicking a browse button,
                        //or it could be a value they had entered previously that used to be valid, but now no longer exists.
                        //In any case, we don't want to throw out the user's text. So, we find the first parent 
                        //directory of Path that exists on disk.
                        //We will set the initial directory to that path, and then set the initial file to
                        //the rest of the path. The user will get an error if the click "OK"m saying that the selected path
                        //doesn't exist, but that's ok. If we didn't do this, and showed the path, if they clicked
                        //OK without making changes we would actually change the selected path, which would be bad.
                        //This way, if the users want's to change the folder, he actually has to change something.
                        string pathToShow;
                        InitializePathDNE(Path,out openFileName.lpstrInitialDir, out pathToShow);
                        pathToShow = pathToShow ?? "";
                        var length = Math.Min(pathToShow.Length, InteropUtil.NumberOfFileChars);
                        pathToShow.CopyTo(0, chars, 0, length);
                    }
                }
                catch
                {
                }

                Marshal.Copy(chars, 0, nativeBuffer, chars.Length);
                
                openFileName.lpstrFile = nativeBuffer;               
                
                if (!AcceptFiles)
                {
                    var str = string.Format("Folders\0*.{0}-{1}\0\0", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
                    filterBuffer = openFileName.lpstrFilter = Marshal.StringToCoTaskMemUni(str);
                }
                else
                {
                    openFileName.lpstrFilter = IntPtr.Zero;    
                }

                openFileName.nMaxCustFilter = 0;
                openFileName.nFilterIndex = 0;
                openFileName.nMaxFile = InteropUtil.NumberOfFileChars;
                openFileName.nMaxFileTitle = 0;
                openFileName.lpstrTitle = Title;
                openFileName.lpfnHook = m_hookDelegate;
                openFileName.templateID = InteropUtil.IDD_CustomOpenDialog;
                openFileName.hInstance = Marshal.GetHINSTANCE(typeof (OpenFileOrFolderDialog).Module);

                openFileName.Flags =
                    InteropUtil.OFN_DONTADDTORECENT |
                    InteropUtil.OFN_ENABLEHOOK |
                    InteropUtil.OFN_ENABLESIZING |
                    InteropUtil.OFN_NOTESTFILECREATE |
                    InteropUtil.OFN_EXPLORER |
                    InteropUtil.OFN_FILEMUSTEXIST |
                    InteropUtil.OFN_PATHMUSTEXIST |
                    InteropUtil.OFN_NODEREFERENCELINKS |
                    InteropUtil.OFN_ENABLETEMPLATE |
                    (ShowReadOnly ? 0 : InteropUtil.OFN_HIDEREADONLY);

                m_useCurrentDir = false;

                var ret = InteropUtil.GetOpenFileNameW(ref openFileName);
                //var extErrpr = InteropUtil.CommDlgExtendedError();
                //InteropUtil.CheckForWin32Error();
                
                if (m_useCurrentDir)
                {
                    Path = m_currentFolder;
                    return true;
                }
                else if (ret)
                {
                    Marshal.Copy(nativeBuffer, chars, 0, chars.Length);
                    var firstZeroTerm = ((IList)chars).IndexOf('\0');
                    if (firstZeroTerm >= 0 && firstZeroTerm <= chars.Length - 1)
                    {
                        Path = new string(chars, 0, firstZeroTerm);
                    }
                }
                return ret;
            }
            finally
            {
                Marshal.FreeCoTaskMem(nativeBuffer);
                if (filterBuffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(filterBuffer);
                }
            }
        }

        private static void InitializePathDNE(string path, out string existingParent, out string initialFileNameText)
        {
            var stack = new Stack<string>();
            existingParent = System.IO.Path.GetDirectoryName(path);
            stack.Push(System.IO.Path.GetFileName(path));

            while (!string.IsNullOrEmpty(existingParent) && !Directory.Exists(existingParent))
            {
                stack.Push(existingParent);
                existingParent = System.IO.Path.GetDirectoryName(existingParent);
            }

            var builder = new StringBuilder();
            bool first = true;
            while (stack.Count > 0)
            {
                if (!first)
                {
                    builder.Append(System.IO.Path.PathSeparator);
                }
                else
                {
                    first = false;
                }
                builder.Append(stack.Pop());
            }
            initialFileNameText = builder.ToString();
        }


        protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lparam)
        {
            switch (unchecked((uint)msg))
            {
                case InteropUtil.WM_INITDIALOG:
                {
                    InitDialog(hWnd);
                    break;
                }
                case InteropUtil.WM_NOTIFY:
                {
                    var notifyData = (InteropUtil.OFNOTIFY)Marshal.PtrToStructure(lparam, typeof(InteropUtil.OFNOTIFY));
                    var results = ProcessNotifyMessage(hWnd, notifyData);
                    if (results != 0)
                    {
                        hWnd.SetWindowLongW(InteropUtil.DWL_MSGRESULT, results);
                        return (IntPtr)results;
                    }
                    break;
                }
                case InteropUtil.WM_SIZE:
                {
                    ResizeCustomControl(hWnd);
                    break;
                }
                case InteropUtil.WM_COMMAND:
                {
                    unchecked
                    {
                        var hParent = hWnd.GetParent().AssumeNonZero();
                        var code = HIGH((uint)wParam);
                        var id = LOW((uint)wParam);
                        if (code == InteropUtil.BN_CLICKED)
                        {
                            switch (id)
                            {
                                case InteropUtil.ID_CUSTOM_CANCEL:
                                {
                                    //The user clicked our custom cancel button. Close the dialog.
                                    hParent.SendMessage(InteropUtil.WM_CLOSE, 0, 0);
                                    break;
                                }
                                case InteropUtil.ID_SELECT:
                                {
                                    var hFileName = hParent.GetDlgItem(InteropUtil.ID_FileNameCombo);
                                    var currentText = (hFileName.GetWindowTextW() ?? "").Trim();
                                    if (currentText == "" && ! String.IsNullOrEmpty(m_currentFolder))
                                    {
                                        //there's not text in the box, so the user must want to select the current folder.
                                        m_useCurrentDir = true;
                                        hParent.SendMessage(InteropUtil.WM_CLOSE, 0, 0);
                                        break;
                                    }
                                    else if (System.IO.Path.IsPathRooted(currentText))
                                    {
                                        if (Directory.Exists(currentText))
                                        {
                                            //the contents of the text box are a rooted path, that points to an existing directory.
                                            //we interpret that to mean that the user wants to select that directory.
                                            m_useCurrentDir = true;
                                            m_currentFolder = currentText;
                                            hParent.SendMessage(InteropUtil.WM_CLOSE, 0, 0);
                                            break;
                                        }
                                    }
                                    else if (! String.IsNullOrEmpty(m_currentFolder) && currentText != "")
                                    {
                                        var combined = System.IO.Path.Combine(m_currentFolder, currentText);
                                        if (Directory.Exists(combined))
                                        {
                                            //the contents of the text box are a relative path, that points to a 
                                            //an existing directory. We interpret the users intent to mean that they wanted
                                            //to select the existing path.
                                            m_useCurrentDir = true;
                                            m_currentFolder = combined;
                                            hParent.SendMessage(InteropUtil.WM_CLOSE, 0, 0);
                                            break;
                                        }
                                    }

                                    //The user has not selected an existing folder.
                                    //So we translate a click of our "Select" button into the OK button and forward the request to the
                                    //open file dialog.
                                    hParent.SendMessage
                                    (
                                        InteropUtil.WM_COMMAND,
                                        (InteropUtil.BN_CLICKED << 16) | InteropUtil.IDOK,
                                        unchecked((uint)hParent.GetDlgItem(InteropUtil.IDOK))
                                    );
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
            return base.HookProc(hWnd, msg, wParam, lparam);
        }

        private void InitDialog(IntPtr hWnd)
        {
            m_hWnd = hWnd;

            var hParent = hWnd.GetParent().AssumeNonZero();
            hParent.SetWindowSubclass(m_openFileSubClassDelegate, 0, 0);
            
            //disable and hide the filter combo box
            var hFilterCombo = hParent.GetDlgItem(InteropUtil.ID_FilterCombo).AssumeNonZero();
            hFilterCombo.EnableWindow(false);
            hParent.SendMessage(InteropUtil.CDM_HIDECONTROL, InteropUtil.ID_FilterCombo, 0);
            hParent.SendMessage(InteropUtil.CDM_HIDECONTROL, InteropUtil.ID_FilterLabel, 0);
            
            //update the file name label
            var hFileNameLabel = hParent.GetDlgItem(InteropUtil.ID_FileNameLabel).AssumeNonZero();
            
            if (FileNameLabel != "")
            {
                hFileNameLabel.SendMessageString(InteropUtil.WM_SETTEXT, 0, FileNameLabel);    
            }

            //find the button controls in the parent
            var hOkButton = hParent.GetDlgItem(InteropUtil.IDOK).AssumeNonZero();
            var hCancelButton = hParent.GetDlgItem(InteropUtil.IDCANCEL).AssumeNonZero();

            //We don't want the accelerator keys for the ok and cancel buttons to work, because
            //they are not shown on the dialog. However, we still want the buttons enabled
            //so that "esc" and "enter" have the behavior they used to. So, we just
            //clear out their text instead.
            hOkButton.SetWindowTextW("");
            hCancelButton.SetWindowTextW("");

            //find our button controls
            var hSelectButton = hWnd.GetDlgItem(InteropUtil.ID_SELECT).AssumeNonZero();
            var hCustomCancelButton = hWnd.GetDlgItem(InteropUtil.ID_CUSTOM_CANCEL).AssumeNonZero();

            //copy the font from the parent's buttons
            hSelectButton.LoadFontFrom(hOkButton);
            hCustomCancelButton.LoadFontFrom(hCancelButton);

            var cancelLoc = hCancelButton.GetWindowPlacement();

            //hide the ok and cancel buttons
            hParent.SendMessage(InteropUtil.CDM_HIDECONTROL, InteropUtil.IDOK, 0);
            hParent.SendMessage(InteropUtil.CDM_HIDECONTROL, InteropUtil.IDCANCEL, 0);

            //expand the file name combo to take up the space left by the OK and cancel buttons.
            var hFileName = hParent.GetDlgItem(InteropUtil.ID_FileNameCombo).AssumeNonZero();
            var fileNameLoc = hFileName.GetWindowPlacement();
            fileNameLoc.Right = hOkButton.GetWindowPlacement().Right;
            hFileName.SetWindowPlacement(ref fileNameLoc);

            var parentLoc = hParent.GetWindowPlacement();

            //subtract the height of the missing cancel button
            parentLoc.Bottom -= (cancelLoc.Bottom - cancelLoc.Top);
            hParent.SetWindowPlacement(ref parentLoc);

            //move the select and custom cancel buttons to the right hand side of the window:

            var selectLoc = hSelectButton.GetWindowPlacement();
            var customCancelLoc = hCustomCancelButton.GetWindowPlacement();
            m_cancelWidth = customCancelLoc.Right - customCancelLoc.Left;
            m_selectWidth = selectLoc.Right - selectLoc.Left;
            m_buttonGap = customCancelLoc.Left - selectLoc.Right;

            var ctrlLoc = hWnd.GetWindowPlacement();
            ctrlLoc.Right = fileNameLoc.Right;

            //ResizeCustomControl(hWnd, fileNameLoc.Right, hCustomCancelButton, hSelectButton);
            ResizeCustomControl(hWnd);
        }

        private void ResizeCustomControl(IntPtr hWnd)
        {
            if (hWnd == m_hWnd)
            {
                var hSelectButton = hWnd.AssumeNonZero().GetDlgItem(InteropUtil.ID_SELECT).AssumeNonZero();
                var hOkButton = hWnd.AssumeNonZero().GetDlgItem(InteropUtil.ID_CUSTOM_CANCEL).AssumeNonZero();

                var hParent = hWnd.GetParent().AssumeNonZero();
                var fileName = hParent.GetDlgItem(InteropUtil.ID_FileNameCombo).AssumeNonZero();

                /*var right = fileName.GetWindowPlacement().Right;
                var top = hSelectButton.GetWindowPlacement().Top;*/

                var rect = new InteropUtil.RECT();
                var selectRect = hSelectButton.GetWindowPlacement();

                rect.top = selectRect.Top;
                rect.bottom = selectRect.Bottom;
                rect.right = fileName.GetWindowPlacement().Right;
                rect.left = rect.right - (m_cancelWidth + m_buttonGap + m_selectWidth);

                ResizeCustomControl(hWnd, rect, hOkButton, hSelectButton);
            }
        }

        private void ResizeCustomControl(IntPtr hWnd, InteropUtil.RECT rect, params IntPtr[] buttons)
        {
            Util.Assume(buttons != null && buttons.Length > 0);

            hWnd.AssumeNonZero();

            var wndLoc = hWnd.GetWindowPlacement();

            wndLoc.Right = rect.right;
            hWnd.SetWindowPlacement(ref wndLoc);

            foreach (var hBtn in buttons)
            {
                int btnRight, btnWidth;

                m_calcPosMap[hBtn.GetDlgCtrlID()](this, rect.right, out btnRight, out btnWidth);

                PositionButton(hBtn, btnRight, btnWidth);
            }

            //see bug # 844
            //We clip hWnd to only draw in the rectangle around our custom buttons.
            //When we supply a custom dialog template to GetOpenFileName(), it adds 
            //an extra HWND to the open file dialog, and then sticks all the controls 
            //in the dialog //template inside the HWND. It then resizes the control 
            //to stretch from the top of the open file dialog to the bottom of the 
            //window, extending the bottom of the window large enough to include the 
            //additional height of the dialog template. This ends up sticking our custom
            //buttons at the bottom of the window, which is what we want.
            //
            //However, the fact that the parent window extends from the top of the open 
            //file dialog was causing some painting problems on Windows XP SP 3 systems. 
            //Basically, because the window was covering the predefined controls on the 
            //open file dialog, they were not getting painted. This results in a blank 
            //window. I tried setting an extended WS_EX_TRANSPARENT style on the dialog, 
            //but that didn't help.
            //
            //So, to fix the problem I setup a window region for the synthetic HWND. 
            //This clips the drawing of the window to only within the region containing
            //the custom buttons, and thus avoids the problem.
            //
            //I'm not sure why this wasn't an issue on Vista. 
            var hRgn = InteropUtil.CreateRectRgnIndirect(ref rect);
            try
            {
                if (hWnd.SetWindowRgn(hRgn, true) == 0)
                {
                    //setting the region failed, so we need to delete the region we created above.
                    hRgn.DeleteObject();
                }
            }
            catch
            {

                if (hRgn != IntPtr.Zero)
                {
                    hRgn.DeleteObject();
                }
            }
        }

        private void PositionButton(IntPtr hWnd, int right, int width)
        {
            hWnd.AssumeNonZero();
            var id = hWnd.GetDlgCtrlID();

            //hWnd.BringWindowToTop();

            var buttonLoc = hWnd.GetWindowPlacement();

            buttonLoc.Right = right;
            buttonLoc.Left = buttonLoc.Right - width;
            hWnd.SetWindowPlacement(ref buttonLoc);
            hWnd.InvalidateRect(IntPtr.Zero, true);

        }

        private int ProcessNotifyMessage(IntPtr hWnd, InteropUtil.OFNOTIFY notifyData)
        {
            switch (notifyData.hdr_code)
            {
                case InteropUtil.CDN_FOLDERCHANGE:
                {
                    var newFolder = GetTextFromCommonDialog(hWnd.GetParent().AssumeNonZero(), InteropUtil.CDM_GETFOLDERPATH);
                    if (m_currentFolder != null && newFolder != null &&  newFolder.PathContains(m_currentFolder))
                    {
                        m_suppressSelectionChange = true;
                    }
                    m_currentFolder = newFolder;
                    var fileNameCombo = hWnd.GetParent().AssumeNonZero().GetDlgItem(InteropUtil.ID_FileNameCombo).AssumeNonZero();
                    if (m_hasDirChangeFired)
                    {
                        fileNameCombo.SetWindowTextW("");
                    }
                    m_hasDirChangeFired = true;    
                    break;
                }               
                case InteropUtil.CDN_FILEOK:
                {
                    if (!AcceptFiles)
                    {
                        return 1;        
                    }
                    break;
                }
                case InteropUtil.CDN_INITDONE:
                {
                    var hParent = hWnd.GetParent();
                    var hFile = hParent.AssumeNonZero().GetDlgItem(InteropUtil.ID_FileNameCombo).AssumeNonZero();
                    hFile.SetFocus();
                    break;
                }
            }

            return 0;
        }

        
        private string GetTextFromCommonDialog(IntPtr hWnd, uint msg)
        {
            string str = null;
            var buffer = Marshal.AllocCoTaskMem(2*InteropUtil.NumberOfFileChars);
            try
            {
                hWnd.SendMessage(msg, InteropUtil.NumberOfFileChars, unchecked((uint)buffer));
                var chars = new char[InteropUtil.NumberOfFileChars];
                Marshal.Copy(buffer, chars, 0,chars.Length);
                var firstZeroTerm = ((IList)chars).IndexOf('\0');
                        
                if (firstZeroTerm >= 0 && firstZeroTerm <= chars.Length - 1)
                {
                    str = new string(chars, 0, firstZeroTerm);
                }

            }
            finally
            {
                Marshal.FreeCoTaskMem(buffer);
            }
            return str;
        }

        private int DefViewSubClass
        (
            IntPtr hWnd,
            uint uMsg,
            IntPtr wParam,
            IntPtr lParam,
            IntPtr uIdSubclass,
            uint dwRefData
        )
        {
            if (uMsg == InteropUtil.WM_NOTIFY)
            {
                var header = (InteropUtil.NMHDR)Marshal.PtrToStructure(lParam, typeof (InteropUtil.NMHDR));
                if (header.code == InteropUtil.LVN_ITEMCHANGED && header.hwndFrom != IntPtr.Zero && header.idFrom == 1)
                {
                    var nmListView = (InteropUtil.NMLISTVIEW)Marshal.PtrToStructure(lParam, typeof(InteropUtil.NMLISTVIEW));
                    var oldSelected = (nmListView.uOldState & InteropUtil.LVIS_SELECTED) != 0;
                    var newSelected = (nmListView.uNewState & InteropUtil.LVIS_SELECTED) != 0;
                    if (!oldSelected && newSelected)
                    {
                        if (!m_suppressSelectionChange)
                        {
                            //the item went from not selected to being selected    
                            //so we want to look and see if the selected item is a folder, and if so
                            //change the text of the item box to be the item on the folder. But, before we do that
                            //we want to make sure that the box isn't currently focused.
                            var hParent = hWnd.GetParent();
                            var hFNCombo = hParent.GetDlgItem(InteropUtil.ID_FileNameCombo);
                            var hFNEditBox = hParent.GetDlgItem(InteropUtil.ID_FileNameTextBox);
                            var hFocus = InteropUtil.GetFocus();

                            if
                            (
                                (hFNCombo == IntPtr.Zero || hFNCombo != hFocus) &&
                                (hFNEditBox == IntPtr.Zero || hFNEditBox != hFocus)
                            )
                            {
                                SetFileNameToSelectedItem(header.hwndFrom, hFNCombo, nmListView.iItem);
                            }
                        }
                        m_suppressSelectionChange = false;
                    }                    
                }
            }
            return hWnd.DefSubclassProc(uMsg, wParam, lParam);
        }

        private void SetFileNameToSelectedItem(IntPtr hListView, IntPtr hFNCombo, int selectedIndex)
        {
            if (selectedIndex >=0)
            {
                var lvitem = new InteropUtil.LVITEM();
                lvitem.mask = InteropUtil.LVIF_TEXT;
                var nativeBuffer = Marshal.AllocCoTaskMem(InteropUtil.NumberOfFileChars * 2);
                for (int i = 0; i < InteropUtil.NumberOfFileChars; ++i)
                {
                    Marshal.WriteInt16(nativeBuffer, i*2, '\0');
                }
                string name;

                try
                {
                    Marshal.WriteInt16(nativeBuffer, 0);
                    lvitem.pszText = nativeBuffer;
                    lvitem.cchTextMax = InteropUtil.NumberOfFileChars;
                    var length = hListView.SendListViewMessage(InteropUtil.LVM_GETITEMTEXT, (uint)selectedIndex, ref lvitem);
                    name = Marshal.PtrToStringUni(lvitem.pszText, (int)length);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(nativeBuffer);
                }

                if (name != null && m_currentFolder != null)
                {
                    try
                    {
                        var path = System.IO.Path.Combine(m_currentFolder, name);
                        if (Directory.Exists(path))
                        {
                            hFNCombo.SetWindowTextW(name);
                        }                        
                    }
                    catch (Exception)
                    {
                    }

                }
            }
        }

        private int OpenFileSubClass
        (
            IntPtr hWnd,
            uint uMsg,
            IntPtr wParam,
            IntPtr lParam,
            IntPtr uIdSubclass,
            uint dwRefData
        )
        {
            switch (uMsg)
            {
                case InteropUtil.WM_PARENTNOTIFY:
                {
                    unchecked
                    {
                        var id = lParam.GetDlgCtrlID();

                        if (LOW((uint)wParam) == InteropUtil.WM_CREATE && (id == InteropUtil.ID_FileList || id == 0))
                        {
                            lParam.SetWindowSubclass(m_defViewSubClassDelegate, 0, 0);
                        }
                    }
                    break;
                }
            }
            return hWnd.DefSubclassProc(uMsg, wParam, lParam);
        }

        private static uint LOW(uint x)
        {
            return x & 0xFFFF;
        }

        private static uint HIGH(uint x)
        {
            return (x & 0xFFFF0000) >> 16;
        }

    }
}
