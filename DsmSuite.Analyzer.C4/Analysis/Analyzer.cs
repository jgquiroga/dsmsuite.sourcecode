using DsmSuite.Analyzer.C4.Settings;
using DsmSuite.Analyzer.Model.Interface;
using DsmSuite.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DsmSuite.Analyzer.C4.Analysis
{
    internal class Analyzer
    {
        private readonly IDsiModel _model;
        private readonly AnalyzerSettings _analyzerSettings;
        private readonly IProgress<ProgressInfo> _progress;
        private readonly Dictionary<string, C4Element> _elements = new Dictionary<string, C4Element>();
        private readonly List<C4Relationship> _relationships = new List<C4Relationship>();

        public Analyzer(IDsiModel model, AnalyzerSettings analyzerSettings, IProgress<ProgressInfo> progress)
        {
            _model = model;
            _analyzerSettings = analyzerSettings;
            _progress = progress;
        }

        private void FindRelationships(JsonElement parent)
        {
            if (!parent.TryGetProperty("relationships", out var relationships))
            {
                return;
            }

            foreach (var relationship in relationships.EnumerateArray())
            {
                var sourceId = relationship.GetProperty("sourceId").GetString();
                var destinationId = relationship.GetProperty("destinationId").GetString();
                var description = relationship.GetProperty("description").GetString();

                Logger.LogUserMessage($"Relationship: {sourceId} -> {destinationId} ({description})");

                _relationships.Add(new C4Relationship { SourceId = sourceId, DestinationId = destinationId, Description = description });
            }
        }

        private void FindPeople(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("people", out var people))
            {
                return;
            }

            foreach (var person in people.EnumerateArray())
            {
                var id = person.GetProperty("id").GetString();
                var name = person.GetProperty("name").GetString();
                var type = person.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "Person";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Person: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindRelationships(person);
            }
        }


        private void FindSoftwareSystems(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("softwareSystems", out var softwareSystems))
            {
                return;
            }

            foreach (var softwareSystem in softwareSystems.EnumerateArray())
            {
                var id = softwareSystem.GetProperty("id").GetString();
                var name = softwareSystem.GetProperty("name").GetString();
                var type = softwareSystem.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "SoftwareSystem";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Software system: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindContainers(softwareSystem, name);

                FindRelationships(softwareSystem);
            }
        }

        private void FindContainers(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("containers", out var containers))
            {
                return;
            }

            foreach (var container in containers.EnumerateArray())
            {
                var id = container.GetProperty("id").GetString();
                var name = container.GetProperty("name").GetString();
                var type = container.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "Container";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Container: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindComponents(container, name);

                FindRelationships(container);
            }
        }

        private void FindComponents(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("components", out var components))
            {
                return;
            }

            foreach (var component in components.EnumerateArray())
            {
                var id = component.GetProperty("id").GetString();
                var name = component.GetProperty("name").GetString();
                var type = component.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "Component";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Component: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindCodeElements(component, name);

                FindRelationships(component);
            }
        }

        private void FindCodeElements(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("codeElements", out var codeElements))
            {
                return;
            }

            foreach (var codeElement in codeElements.EnumerateArray())
            {
                var id = codeElement.GetProperty("id").GetString();
                var name = codeElement.GetProperty("name").GetString();
                var type = codeElement.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "CodeElement";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Code element: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindRelationships(codeElement);
            }
        }

        private void FindChildElements(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("children", out var children))
            {
                return;
            }

            foreach (var child in children.EnumerateArray())
            {
                var id = child.GetProperty("id").GetString();
                var name = child.GetProperty("name").GetString();
                var type = child.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "Child";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Child: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                // Recursively add children
                FindChildElements(child, name);

                FindRelationships(child);
            }
        }

        private void FindDeploymentNodes(JsonElement parent, string parentName)
        {
            if (!parent.TryGetProperty("deploymentNodes", out var deploymentNodes))
            {
                return;
            }

            foreach (var deploymentNode in deploymentNodes.EnumerateArray())
            {
                var id = deploymentNode.GetProperty("id").GetString();
                var name = deploymentNode.GetProperty("name").GetString();
                var type = deploymentNode.TryGetProperty("type", out var typeElement) ? typeElement.GetString() : "DeploymentNode";

                if (parentName != null)
                {
                    name = $"{parentName}.{name}";
                }

                Logger.LogUserMessage($"Deployment node: {name}");

                _model.AddElement(name, type, null);
                _elements.Add(id, new C4Element { Id = id, Name = name });

                FindChildElements(deploymentNode, name);

                FindRelationships(deploymentNode);
            }
        }

        private void RegisterRelationships()
        {
            foreach (var relationship in _relationships)
            {
                if (_elements.TryGetValue(relationship.SourceId, out var sourceElement) && _elements.TryGetValue(relationship.DestinationId, out var destinationElement))
                {
                    _model.AddRelation(sourceElement.Name, destinationElement.Name, relationship.Description, 1, null);
                }
            }
        }

        public void Analyze()
        {
            var workspace = _analyzerSettings.Input.Workspace;

            FileInfo fileInfo = new FileInfo(workspace);
            if (!fileInfo.Exists)
            {
                Logger.LogError($"Workspace file '{workspace}' does not exist.");
            }

            using (FileStream stream = fileInfo.Open(FileMode.Open))
            {
                // Read a C4 json model from the workspace file
                JsonElement workspaceFile = JsonDocument.Parse(stream).RootElement;
                var model = workspaceFile.GetProperty("model");

                FindPeople(model, null);

                FindSoftwareSystems(model, null);

                FindDeploymentNodes(model, null);

                RegisterRelationships();
            }
        }
    }

    public class C4Element
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class C4Relationship
    {
        public string SourceId { get; set; }

        public string DestinationId { get; set; }

        public string Description { get; set; }
    }
}
