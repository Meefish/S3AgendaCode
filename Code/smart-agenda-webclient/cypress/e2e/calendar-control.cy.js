import { clear } from "@testing-library/user-event/dist/clear";
describe('Calendar Functionality Test', () => {
  beforeEach(() => {
    cy.visit('http://localhost:3000/');
    cy.get('input#email').type('user@example.com');
    cy.get('input#password').type('password123!');
    cy.get('form').submit();
  });	

  afterEach(() => {
    cy.get('button').contains('Logout').click();
  });

  it('checks if calendar displays current month on startup', () => {
    cy.get('td').find('.current-day').should('have.length', 1);
  });

  it('checks if previous button displays previous month', () => {
    let initialMonthYear;
    cy.get('.current-month-year-display').then(($span) => {  
      initialMonthYear = $span.text();
    });

    cy.get('button').contains('Previous').click();

    cy.get('.current-month-year-display').should(($span) => {
      expect($span.text()).not.to.eq(initialMonthYear);
    });
  });

  it('checks if today button displays current month', () => {
    
    cy.get('button').contains('Next').click();
    cy.get('button').contains('Today').click();

    cy.get('td').find('.current-day').should('have.length', 1);
  });

  it('checks if next button displays next month', () => {
    let initialMonthYear;
    cy.get('.current-month-year-display').then(($span) => {  
      initialMonthYear = $span.text();
    });

    cy.get('button').contains('Next').click();

    cy.get('.current-month-year-display').should(($span) => {
      expect($span.text()).not.to.eq(initialMonthYear);
    });
  });
});    

describe('Calendar Add Task Functionality', () => {

  beforeEach(() => {
    cy.visit('http://localhost:3000/');
    cy.get('input#email').type('user@example.com');
    cy.get('input#password').type('password123!');
    cy.get('form').submit();
    cy.get('button').contains('Next').click();
    });

  afterEach(() => {
    cy.get('button').contains('Logout').click();
    });
  
  it('checks if Task can be added containing low priority', () => {
    cy.get('td').contains('14').click('top');
    cy.get('input#taskText').type('Walking the dog');
    cy.get('input#taskTime').type('12:30');
    cy.get('select#taskPriority').select('Low');
    cy.get('input#taskStatus').uncheck();  
    cy.get('.popup button').contains('Save Task').click();
    cy.get('.task-bar').should('contain', 'Walking the dog 12:30');

    //For cleanup
    cy.DeleteTask('Walking the dog', '12:30');
  });

  it('checks if Task can be added containing medium priority', () => {
    cy.get('td').contains('14').click('top');
    cy.get('input#taskText').type('Yoga class');
    cy.get('input#taskTime').type('17:30');
    cy.get('select#taskPriority').select('Medium'); 
    cy.get('input#taskStatus').uncheck();  
    cy.get('.popup button').contains('Save Task').click();
    cy.get('.task-bar').should('contain', 'Yoga class 17:30');

    //For cleanup
    cy.DeleteTask('Yoga class', '17:30');
  });

  it('checks if Task can be added containing high priority', () => {
    cy.get('td').contains('14').click('top');
    cy.get('input#taskText').type('Preparing dinner');
    cy.get('input#taskTime').type('19:30');
    cy.get('select#taskPriority').select('High');
    cy.get('input#taskStatus').uncheck();  
    cy.get('.popup button').contains('Save Task').click();
    cy.get('.task-bar').should('contain', 'Preparing dinner 19:30');

    //For cleanup
    cy.DeleteTask('Preparing dinner', '19:30');
  });

  it('checks if Task can be added containing urgent priority', () => {
    cy.get('td').contains('14').click('top');
    cy.get('input#taskText').type('Sleep schedule');
    cy.get('input#taskTime').type('20:45');
    cy.get('select#taskPriority').select('Urgent'); 
    cy.get('input#taskStatus').uncheck();  
    cy.get('.popup button').contains('Save Task').click();
    cy.get('.task-bar').should('contain', 'Sleep schedule 20:45');

    //For cleanup
    cy.DeleteTask('Sleep schedule', '20:45');
  });
});

describe('Calendar Priority Color Check', () => {

  beforeEach(() => {
    cy.visit('http://localhost:3000/');
    cy.get('input#email').type('user@example.com');
    cy.get('input#password').type('password123!');
    cy.get('form').submit();
    cy.get('button').contains('Next').click();
    });

  afterEach(() => {
    cy.get('button').contains('Logout').click();
    });


  it('checks if Low priority is green', () => {
    //Setup
    cy.AddTask('Walking the dog', '12:30', 'Low', false);

    cy.get('.task-bar').contains('Walking the dog 12:30')
    .should('have.attr', 'style')
    .and('include', 'background-color: green');

    //For cleanup
    cy.DeleteTask('Walking the dog', '12:30');
  });

  it('checks if Medium priority is yellow', () => {
    //Setup
    cy.AddTask('Yoga class', '17:30', 'Medium', false);

    cy.get('.task-bar').contains('Yoga class 17:30')
    .should('have.attr', 'style')
    .and('include', 'background-color: yellow');

    //For cleanup
    cy.DeleteTask('Yoga class', '17:30');
  });

  it('checks if High priority is orange', () => {
    //Setup
    cy.AddTask('Preparing dinner', '19:30', 'High', false);

    cy.get('.task-bar').contains('Preparing dinner 19:30')
    .should('have.attr', 'style')
    .and('include', 'background-color: orange');

    //For cleanup
    cy.DeleteTask('Preparing dinner', '19:30');
  });

  it('checks if Urgent priority is red', () => {
    //Setup
    cy.AddTask('Sleep schedule', '20:45', 'Urgent', false);

    cy.get('.task-bar').contains('Sleep schedule 20:45')
    .should('have.attr', 'style')
    .and('include', 'background-color: red');

    //For cleanup
    cy.DeleteTask('Sleep schedule', '20:45');
  });

});

describe('Calendar Update Task Functionality', () => {

    beforeEach(() => {
      cy.visit('http://localhost:3000/');
      cy.get('input#email').type('user@example.com');
      cy.get('input#password').type('password123!');
      cy.get('form').submit();
      cy.get('button').contains('Next').click();
  
      cy.AddTask('Walking the dog', '12:30', 'Low', false);
      });
  
    afterEach(() => {
      cy.get('button').contains('Logout').click();
      });

  it('checks if tasks name can be edited', () => {
    cy.get('.task-bar').contains('Walking the dog 12:30').click();
    cy.get('input#taskText').clear().type('Walking the Basenji');
    cy.get('.popup button').contains('Save Task').click();

    cy.get('.task-bar').should('contain', 'Walking the Basenji 12:30');

    //For cleanup
    cy.DeleteTask('Walking the Basenji', '12:30');
  });

  it('checks if tasks time can be edited', () => {
    cy.get('.task-bar').contains('Walking the dog 12:30').click();
    cy.get('input#taskTime').clear().type('18:00');
    cy.get('.popup button').contains('Save Task').click();

    cy.get('.task-bar').should('contain', 'Walking the dog 18:00');

    //For cleanup
    cy.DeleteTask('Walking the dog', '18:00');;
  });

  it('checks if tasks priority can be edited and contains right color', () => {
    cy.get('.task-bar').contains('Walking the dog 12:30').click();
    cy.get('select#taskPriority').select('Urgent'); 
    cy.get('.popup button').contains('Save Task').click();

    cy.get('.task-bar').should('contain', 'Walking the dog 12:30');
    cy.get('.task-bar').contains('Walking the dog 12:30')
    .should('have.attr', 'style')
    .and('include', 'background-color: red');

    //For cleanup
    cy.DeleteTask('Walking the dog', '12:30');
  });
});


describe('Calendar Delete Task Functionality', () => {

  beforeEach(() => {
    cy.visit('http://localhost:3000/');
    cy.get('input#email').type('user@example.com');
    cy.get('input#password').type('password123!');
    cy.get('form').submit();
    cy.get('button').contains('Next').click();
  
    cy.AddTask('Walking the dog', '12:30', 'Low', false);
    });

    afterEach(() => {
    cy.get('button').contains('Logout').click();
    });
    

  it('checks if tasks can be deleted', () => {

    cy.get('.task-bar').contains('Walking the dog 12:30').click();
    cy.get('.popup button').contains('Delete Task').click();
  });
});
