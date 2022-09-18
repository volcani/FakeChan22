﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace FakeChan22.Tasks
{
    class SubTaskCommentGen
    {
        object lockObj = new object();
        string CommentFilename = @"comment.xml";

        BlockingCollection<XmlElement> CommQue = null;

        XmlDocument LogXmlDocument;
        XmlElement LogDocumentRoot;

        DateTime Time19700101 = new DateTime(1970, 1, 1);

        uint CommentNumberCount = 0;

        public string CommentSavePath { get; private set; }

        public SubTaskCommentGen()
        {
            CommQue = new BlockingCollection<XmlElement>();
        }

        public void TaskStart(string xmlpath)
        {
            CommentSavePath = xmlpath;

            LogXmlDocument = new XmlDocument();
            LogDocumentRoot = LogXmlDocument.CreateElement("log");

            LogXmlDocument.AppendChild(LogDocumentRoot);

            CommentNumberCount = 0;

            try
            {
                LogXmlDocument.Save(CommentSavePath + CommentFilename);
            }
            catch (Exception)
            {
                //
            }
        }

        public void TaskStop()
        {
            LogDocumentRoot = null;
            LogXmlDocument = null;
        }

        public void AddComment(string commentText, string service, string handle, string cv)
        {
            XmlElement comment = LogXmlDocument.CreateElement("comment");
            comment.SetAttribute("no", CommentNumberCount.ToString());
            comment.SetAttribute("time", ((int)(DateTime.UtcNow.Subtract(Time19700101)).TotalSeconds).ToString());
            comment.SetAttribute("service", service);
            comment.SetAttribute("cv", cv);
            comment.SetAttribute("handle", handle);
            comment.SetAttribute("owner", "0");
            comment.InnerText = commentText;
            LogDocumentRoot.AppendChild(comment);
            CommQue.Add(comment);

            CommentNumberCount++;

            lock(lockObj)
            {
                int cnt = LogDocumentRoot.ChildNodes.Count;

                if (500 < cnt)
                {
                    LogDocumentRoot.RemoveChild(LogDocumentRoot.FirstChild);
                }

                try
                {
                    LogXmlDocument.Save(CommentSavePath + CommentFilename);
                }
                catch (Exception)
                {
                    //
                }
            }
        }

    }
}