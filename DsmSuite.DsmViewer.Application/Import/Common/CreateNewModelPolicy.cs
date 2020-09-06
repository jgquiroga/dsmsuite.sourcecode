﻿using System;
using DsmSuite.Common.Model.Interface;
using DsmSuite.Common.Util;
using DsmSuite.DsmViewer.Model.Interfaces;

namespace DsmSuite.DsmViewer.Application.Import.Common
{
    public class CreateNewModelPolicy : IImportPolicy
    {
        private readonly IDsmModel _dsmModel;

        public CreateNewModelPolicy(IDsmModel dsmmodel)
        {
            _dsmModel = dsmmodel;
            _dsmModel.Clear();
        }

        public IMetaDataItem ImportMetaDataItem(string group, string name, string value)
        {
            return _dsmModel.AddMetaData(group, name, value);
        }

        public IDsmElement ImportElement(string fullname, string name, string type, IDsmElement parent)
        {
            IDsmElement element = _dsmModel.GetElementByFullname(fullname);
            if (element == null)
            {
                int? parentId = parent?.Id;
                element = _dsmModel.AddElement(name, type, parentId);
            }
            return element;
        }

        public IDsmRelation ImportRelation(int consumerId, int providerId, string type, int weight)
        {
            IDsmElement consumer = _dsmModel.GetElementById(consumerId);
            IDsmElement provider = _dsmModel.GetElementById(providerId);

            return _dsmModel.AddRelation(consumer, provider, type, weight);
        }

        public void FinalizeImport(IProgress<ProgressInfo> progress)
        {
            _dsmModel.AssignElementOrder();
        }
    }
}
