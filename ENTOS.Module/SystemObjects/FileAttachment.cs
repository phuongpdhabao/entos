﻿using System;
using System.ComponentModel;
using System.IO;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace ENTOS.Module.SystemObjects
{
    [ModelDefault("Caption", "Tập đính kèm")]
    [DefaultProperty("FileName")]    
    [Serializable()]
    public partial class FileAttachment : XPLiteObject, IFileData, IEmptyCheckable, INotifyPropertyChanged
    {
        
        //private string sourceFile;
        private string oldFile;
        private bool IsEdit = false;

        public FileAttachment(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            SaveFolder = Module.Helpers.ParameterHelper.GetValue(Session, "SaveFolder");
            base.AfterConstruction();
        }

        [DevExpress.Xpo.Key(AutoGenerate = true), Browsable(false)]
        public Guid Oid { get; set; }

        private string fileName;
        [ImmediatePostData]
        public string FileName
        {
            get
            {
                return fileName;
            }

            set { fileName = value; }
        }

        public string FullFileName()
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                    return SaveFolder + "\\" + Oid + "_" + fileName;
            }
            catch (Exception ex)
            {
                
            }
            
            return "";
        }

        [Browsable(false)]
        public string SaveFolder { get; set; }
        public bool IsEmpty
        {
            get { return String.IsNullOrEmpty(FileName); }
        }

        public int Size
        {
            get
            {
                if (Content != null)
                {
                    return Content.Length;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Byte[] GetContent()
        {
            return Content;
        }
        private Byte[] Content
        {
            get
            {
                if (_content == null)
                {
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        string fullFileName = FullFileName();
                        if (File.Exists(fullFileName))
                        {
                            oldFile = fullFileName;
                            _content = File.ReadAllBytes(FullFileName());
                        }
                        else
                        {
                            //FileName += "(Đã xóa) - " + FileName;
                        }
                    }
                }
                return _content;
            }
            set { _content = value; }
        }
        private Byte[] _content;

        public void Clear()
        {
            Content = null;
            FileName = "";
            IsEdit = true;
            this.OnChanged("FileName");
        }

        public void SetContent(string file, Byte[] content)
        {
            FileName = file;
            Content = content;
            IsEdit = true;
            OnChanged("FileName");
        }
        public void LoadFromStream(string fileName, Stream stream)
        {
            FileName = fileName;     
            Byte[] bytes = new Byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            Content = bytes;
            IsEdit = true;
            this.OnChanged("FileName");          
        }

        public void SaveToStream(Stream stream)
        {
            
            if (String.IsNullOrEmpty(FileName))
            {
                throw new InvalidOperationException();
            }
            if (Content != null)
            {
                stream.Write(Content, 0, Size);
                stream.Flush();
            }            
        }
        public override String ToString()
        {
            return FullFileName();
        }
        

        protected override void OnLoaded()
        {
            base.OnLoaded();
            //if (!string.IsNullOrEmpty(FileName))
            //{
            //    string fullFileName = FullFileName();
            //    if (File.Exists(fullFileName))
            //    {
            //        oldFile = fullFileName;
            //        Content = File.ReadAllBytes(FullFileName());
            //    }
            //    else
            //    {
            //        //FileName += "(Đã xóa) - " + FileName;
            //    }
            //}
            
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();
        }


        protected override void OnDeleting()
        {
            if (!string.IsNullOrEmpty(FullFileName()))
            {
                if (File.Exists(FullFileName()))
                {
                    File.Delete(FullFileName());
                }
            }
            base.OnDeleting();
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (IsEdit)
            {
                if (!string.IsNullOrEmpty(oldFile) && !oldFile.Equals(FullFileName()))
                {
                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                    }
                }
                if (!string.IsNullOrEmpty(FileName) && !IsDeleted)
                {
                    File.WriteAllBytes(FullFileName(), Content);
                }
            }
            if (IsDeleted && !string.IsNullOrEmpty(FullFileName()))
            {
                if (File.Exists(FullFileName()))
                {
                    File.Delete(FullFileName());
                }
            }
                        
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }

}
