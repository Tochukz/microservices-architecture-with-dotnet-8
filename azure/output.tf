output "servicebus_namespace_connection_string" {
  value     = azurerm_servicebus_namespace_authorization_rule.root.primary_connection_string
  sensitive = true
}

output "queue_connection_string" {
  value     = azurerm_servicebus_queue_authorization_rule.cart_queue_rw.primary_connection_string
  sensitive = true
}

# output "servicebus_namespace_endpoint" {
#   value     = azurerm_servicebus_namespace.simple_bus.endpoint
# }

# output "servicebus_queue_id" {
#   value     = azurerm_servicebus_queue.queue.id
# }
