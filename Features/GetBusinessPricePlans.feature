Feature: GetBusinessPricePlans
In Order to create business price plans
As a client of the Web API
I want to create different subscriptions

Scenario: Check SubscriptionVolume
Given request to post sphere Business Price plans
When user client post the referenceCode to the website
Then Lists the different subscription volumes are created


Scenario: Check SubscriptionPeriod
Given request to post sphere Business Price plans
When user client post the referenceCode to the website
Then Lists the different subscription Period are created