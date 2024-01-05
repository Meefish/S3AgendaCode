Cypress.Commands.add('AddTask', (taskName, taskTime, taskPriority, taskStatus) => {
  cy.get('td').contains('14').click('top');
  cy.get('input#taskText').clear().type(taskName);
  cy.get('input#taskTime').clear().type(taskTime);
  cy.get('select#taskPriority').select(taskPriority);
  if (taskStatus) {
    cy.get('input#taskStatus').check();
  } else {
    cy.get('input#taskStatus').uncheck();
  }
  cy.get('.popup button').contains('Save Task').click();
});

Cypress.Commands.add('DeleteTask', (taskName, taskTime) => {
  cy.get('.task-bar').contains(`${taskName} ${taskTime}`).click();
  cy.get('.popup button').contains('Delete Task').click();
});