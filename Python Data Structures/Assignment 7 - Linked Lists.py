"""
File: a7.py
Assignment 7

Define a length function.
Define a printStructure function.
Define an insert function.
Test the above functions and the Node class.
"""

from node import Node

def length(head):
    """Returns the number of items in the linked structure
    referred to by head."""
    
    probe = head
    count = 1
    
    if probe.next != None:
        count =  1 + length(probe.next)
    
    return count
    
def insert(newItem, head):
    """Inserts newItem at the correct position (ascending order) in
    the linked structure referred to by head.
    Returns a reference to the new structure."""
    if head == None:
        #if structure is empty
        head = Node(newItem)
    else:
        cursor = head
        for i in range(length(head)):
            cursor = cursor.next

        myNode = Node(newItem, head.next)
        head.next = myNode

    return head

def printStructure(head):
    """Prints the items in the structure referred to by head."""
    if head != None:
        print(head.data)
        printStructure(head.next)
        

def main():
    """Gets words from the user and inserts in the
    structure referred to by head."""
    
    head = None
    userInput = input('Please enter a word (or just hit enter to end): ')
    while userInput != '':
        head = insert(userInput, head)
        userInput = input('Please enter a word (or just hit enter to end): ')
    print('The structure contains', length(head), 'items:')
    printStructure(head)

if __name__ == "__main__": main()
