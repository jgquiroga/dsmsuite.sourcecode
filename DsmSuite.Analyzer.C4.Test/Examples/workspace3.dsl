workspace {

    !identifiers hierarchical

    model {
        s = softwareSystem "Software System" {
            app1 = container "Application 1" {
                group "shared-library.jar" {
                    !include shared-library.dsl
                }

                c = component "Component" {
                    -> loggingComponent "Writes logs using"
                }
            }

            app2 = container "Application 2" {
                group "shared-library.jar" {
                    !include shared-library.dsl
                }

                c = component "Component" {
                    -> loggingComponent "Writes logs using"
                }
            }
        }
    }

    views {
    }
    
}