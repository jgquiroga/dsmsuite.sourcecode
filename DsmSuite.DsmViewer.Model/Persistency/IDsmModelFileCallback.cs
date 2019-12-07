﻿using System.Collections.Generic;
using DsmSuite.DsmViewer.Model.Interfaces;

namespace DsmSuite.DsmViewer.Model.Persistency
{
    public interface IDsmModelFileCallback
    {
        IDsmMetaDataItem ImportMetaDataItem(string groupName, string name, string value);
        IDsmElement ImportElement(int id, string name, string type, int order, bool expanded, int? parentId);
        IDsmRelation ImportRelation(int consumerId, int providerId, string type, int weight);

        IEnumerable<string> GetMetaDataGroups();
        IEnumerable<IDsmMetaDataItem> GetMetaDataGroupItems(string groupName);
        IEnumerable<IDsmElement> GetRootElements();
        int GetElementCount();
        IEnumerable<IDsmRelation> GetRelations();
        int GetRelationCount();
    }
}