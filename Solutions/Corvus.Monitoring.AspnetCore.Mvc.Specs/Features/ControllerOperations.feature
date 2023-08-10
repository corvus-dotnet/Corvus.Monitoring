Feature: Controller Operations

As a developer
When I add the ObservableActionMethodsAttribute to my controller
And implement the IHaveObservableActionMethods interface
I want my controller actions to have spans created for them automatically

Scenario: Basic instrumentation of controller action methods
	Given my controller implements the IHaveObservableActionMethods interface and has the ObservableActionMethodsAttribute applied to it
	When I make a web request that is fulfilled by that controller
	Then an IOperationInstance should have been created for the action execution
	And route data should have been added to the IOperationInstance for the action execution
	And operation properties added in the action method should have been added to the IOperationInstance for the action execution
	And an IOperationInstance should have been created for the result execution
	And all IOperationInstances have been disposed