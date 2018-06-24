import Crypto
import math
import random
from Crypto.Util import number
import sys
sys.setrecursionlimit(1000000)

# Sources and Tutorials:

# I found this website very helpful for creating the
# encryption and decryption functions, so I want to credit it:
# https://inventwithpython.com/hacking/chapter24.html

# I used this trial division algorithm for the factoring: https://en.wikipedia.org/wiki/Trial_division
# I probably should have spent more time finding a more method efficient though.


#Factor with Trial Division

def factor(n):
    n = int(n)
    prime_list = []
    while n % 2 == 0:
        prime_list.append(2)
        n /= 2
    factor = 3
    while n > 1:
        if (n % factor == 0):
            prime_list.append(factor)
            n /= factor
        else:
            factor += 2
    return prime_list
        
        
    
#Extended Euclid Algoritm

def extendedEuclid(a, b):
    if a == 0:
        return (b, 0, 1)
    else:
        gcd, x, y = extendedEuclid(b % a, a)
        return (gcd, y - (b // a) * x, x)


# Mulitplicative Inverse

def multInv(e, totient):
    multInverseTuple = extendedEuclid(e, totient)
    # multInverseTuple[0] is the gdc
    if multInverseTuple[0] != 1:
        raise Exception('inverse does not exist')
    else:
        return (multInverseTuple[1] % totient)
    

# Generates the keys.

def CreateKey():

    length = 2048
    p = int(number.getPrime(length))
    q = int(number.getPrime(length))
    n = p * q
    totient = (p-1)*(q-1)

    e = 65537 # use traditional value of e as a starting point

    d = multInv(e, totient)
    

    return n, e, d


# Divide the message into blocks

def CreateBlocks(m):
    msgBytes = m.encode('ascii') # message as bytes
    blocksNum = []               #list stores integer reprentations of each block
    start = 0
    blocksize = 2048 # same size as key
    
    for start in range(0,len(msgBytes), blocksize):
        blockNum = 0
        for i in range(start, min(start + blocksize, len(msgBytes))):
            blockNum += msgBytes[i] * (256 ** (i % 2048))

        blocksNum.append(blockNum)
            
    
    return blocksNum

# Combine blocks into the message for decryption

def BlocksToMessage(blockNums, length):
    message = []
    for blockNum in blockNums:
        tempMsg = []
        for i in range(2048-1, -1, -1):
            if len(message) + i < length:
                asciiValue = blockNum // (256 ** i)
                blockNum = blockNum % (256 ** i)
                tempMsg.insert(0, chr(asciiValue))
                
        message.extend(tempMsg)

    return ''.join(message)
    
                
    
# Encrypt message

def Encrypt(m, e, n):
    # Translate the string into bytes:
    
    cipherText = [] #list to store final ciphertext
    blocks = CreateBlocks(m)

    
    for i in range(len(blocks)):
        cipherText.append(pow(blocks[i], e, n))
    return cipherText

# Decrypt Message

def Decrypt(c, d, n, length):
    plaintext = []

    for block in c:
        plaintext.append(pow(block, d, n))

    return BlocksToMessage(plaintext, length)
    


def main():
    (n, e, d) = CreateKey()
    plaintext = input("Enter a message to be encrypted: ")
    ciphertext = Encrypt(str(plaintext), e, n)
    decrypted = Decrypt(ciphertext, d, n, len(plaintext))
    print("\nCipher Text: ", *ciphertext)
    print("\nDecrypted Plaintext: ", decrypted)

    print("\nFactoring Test: ")
    factorNum = input("\nEnter a number to factor: ")
    smallestPrime = factor(factorNum)
    print("The prime factors of ", factorNum, "are", smallestPrime, ".")  
    
    


main()
