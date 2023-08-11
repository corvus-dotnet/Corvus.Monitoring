Feature: Instrumenting controller actions

As a developer
When I add the ObservableActionMethodsAttribute to my controller
I want my controller actions to have spans created for them automatically
And I want to be able to add custom data to those spans

Scenario: Basic instrumentation of controller action methods
	Given my controller has the ObservableActionMethodsAttribute applied to it
	When I make a web request that is fulfilled by that controller
	Then an IOperationInstance should have been created for the action execution
	And route data should have been added to the IOperationInstance for the action execution
	And operation properties added in the action method should have been added to the IOperationInstance for the action execution
	And an IOperationInstance should have been created for the result execution
	And operation properties added in the view should have been added to the IOperationInstance for the result execution
	And all IOperationInstances have been disposed