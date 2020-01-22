# Wormy (a Nibbles clone)
# By Al Sweigart al@inventwithpython.com
# http://inventwithpython.com/pygame
# Released under a "Simplified BSD" license

import random, pygame, sys,math
from pygame.locals import *

class Bullet:
    def __init__(self, direction, position):
        self.direction = direction
        self.position = position


FPS = 1
WINDOWWIDTH = 680
WINDOWHEIGHT = 520
CELLSIZE = 20
RADIUS = math.floor(CELLSIZE/2.5)
assert WINDOWWIDTH % CELLSIZE == 0, "Window width must be a multiple of cell size."
assert WINDOWHEIGHT % CELLSIZE == 0, "Window height must be a multiple of cell size."
CELLWIDTH = int(WINDOWWIDTH / CELLSIZE)
CELLHEIGHT = int(WINDOWHEIGHT / CELLSIZE)

#             R    G    B
WHITE     = (255, 255, 255)
BLACK     = (  0,   0,   0)
RED       = (255,   0,   0)
GREEN     = (  0, 255,   0)
DARKGREEN = (  0, 155,   0)
DARKGRAY  = ( 40,  40,  40)
YELLOW = (255,255,0)
BGCOLOR = BLACK

UP = 'up'
DOWN = 'down'
LEFT = 'left'
RIGHT = 'right'

HEAD = 0 # syntactic sugar: index of the worm's head

def main():
    global FPSCLOCK, DISPLAYSURF, BASICFONT

    pygame.init()
    FPSCLOCK = pygame.time.Clock()
    DISPLAYSURF = pygame.display.set_mode((WINDOWWIDTH, WINDOWHEIGHT))
    BASICFONT = pygame.font.Font('freesansbold.ttf', 18)
    pygame.display.set_caption('Snakes')

    showStartScreen()
    while True:
        runGame()
        showGameOverScreen()


def runGame():
    # Set a random start point.
    startx = random.randint(5, CELLWIDTH - 6)
    starty = random.randint(5, CELLHEIGHT - 6)
    wormCoords = [{'x': startx + 2,     'y': starty},
                  {'x': startx + 2, 'y': starty - 1},
                  {'x': startx + 2, 'y': starty - 2}]

    worm2Coords = [{'x': startx - 2, 'y': starty},
                  {'x': startx - 2, 'y': starty - 1},
                  {'x': startx - 2, 'y': starty - 2},
                  {'x': startx - 2, 'y': starty - 3},
                  {'x': startx - 2, 'y': starty - 4},
                  {'x': startx - 2, 'y': starty - 5},
                  {'x': startx - 2, 'y': starty - 6}]
    bullets = []
    stones = []


    direction = DOWN
    direction2 = DOWN

    # Start the apple in a random place.
    apples = []
    i = 0
    while i < 10:
        apples.append(getRandomLocation())
        i+=1

    while True: # main game loop

        for event in pygame.event.get(): # event handling loop
            if event.type == QUIT:
                terminate()
            elif event.type == KEYDOWN:
                if (event.key == K_LEFT) and direction != RIGHT:
                    direction = LEFT
                elif (event.key == K_RIGHT) and direction != LEFT:
                    direction = RIGHT
                elif (event.key == K_UP) and direction != DOWN:
                    direction = UP
                elif (event.key == K_DOWN) and direction != UP:
                    direction = DOWN
                elif event.key == K_RSHIFT :
                    if direction == UP:
                        bullet = Bullet(direction, {'x': wormCoords[HEAD]['x'], 'y': wormCoords[HEAD]['y'] - 1})
                    elif direction == DOWN:
                        bullet = Bullet(direction, {'x': wormCoords[HEAD]['x'], 'y': wormCoords[HEAD]['y'] + 1})
                    elif direction == LEFT:
                        bullet = Bullet(direction, {'x': wormCoords[HEAD]['x'] - 1, 'y': wormCoords[HEAD]['y']})
                    elif direction == RIGHT:
                        bullet = Bullet(direction, {'x': wormCoords[HEAD]['x'] + 1, 'y': wormCoords[HEAD]['y']})
                    bullets.append(bullet)

                if (event.key == K_a) and direction2 != RIGHT:
                    direction2 = LEFT
                elif (event.key == K_d) and direction2 != LEFT:
                    direction2 = RIGHT
                elif (event.key == K_w) and direction2 != DOWN:
                    direction2 = UP
                elif (event.key == K_s) and direction2 != UP:
                    direction2 = DOWN
                elif event.key == K_LSHIFT:
                    if direction == UP:
                        bullet = Bullet(direction, {'x': worm2Coords[HEAD]['x'], 'y': worm2Coords[HEAD]['y'] - 1})
                    elif direction == DOWN:
                        bullet = Bullet(direction, {'x': worm2Coords[HEAD]['x'], 'y': worm2Coords[HEAD]['y'] + 1})
                    elif direction == LEFT:
                        bullet = Bullet(direction, {'x': worm2Coords[HEAD]['x'] - 1, 'y': worm2Coords[HEAD]['y']})
                    elif direction == RIGHT:
                        bullet = Bullet(direction, {'x': worm2Coords[HEAD]['x'] + 1, 'y': worm2Coords[HEAD]['y']})
                    bullets.append(bullet)
                elif event.key == K_ESCAPE:
                    terminate()



            # check if bullets have hit a worm
        for bullet in bullets:
            if (bullet.position in wormCoords):
                index = wormCoords.index(bullet.position)
                if (index == HEAD):
                    return # game over
                else:
                    stones.extend(wormCoords[index:])
                    wormCoords = wormCoords[:index]
        for bullet in bullets:
            if (bullet.position in worm2Coords):
                index = worm2Coords.index(bullet.position)
                if (index == HEAD):
                    return # game over
                else:
                    stones.extend(worm2Coords[index:])
                    worm2Coords = worm2Coords[:index]

        # check if the worm has hit itself or the edge
        if wormCoords[HEAD]['x'] == -1 or wormCoords[HEAD]['x'] == CELLWIDTH or wormCoords[HEAD]['y'] == -1 or wormCoords[HEAD]['y'] == CELLHEIGHT:
            return # game over
        if wormCoords[HEAD] in wormCoords[1:]:
            return
        if wormCoords[HEAD] in worm2Coords[0:]:
            return
        if worm2Coords[HEAD] in worm2Coords[1:]:
            return
        if worm2Coords[HEAD] in wormCoords[0:]:
            return
        if worm2Coords[HEAD] in stones[0:]:
            return
        if wormCoords[HEAD] in stones[0:]:
            return

                # check if worm has eaten an apply
        if wormCoords[HEAD] in apples:
            apples[apples.index(wormCoords[HEAD])] = getRandomLocation()
        else:
            del wormCoords[-1]  # remove worm's tail segment
        if worm2Coords[HEAD] in apples:
            apples[apples.index(worm2Coords[HEAD])] = getRandomLocation()
        else:
            del worm2Coords[-1]


        # move the worm by adding a segment in the direction it is moving
        if direction == UP:
            newHead = {'x': wormCoords[HEAD]['x'], 'y': wormCoords[HEAD]['y'] - 1}
        elif direction == DOWN:
            newHead = {'x': wormCoords[HEAD]['x'], 'y': wormCoords[HEAD]['y'] + 1}
        elif direction == LEFT:
            newHead = {'x': wormCoords[HEAD]['x'] - 1, 'y': wormCoords[HEAD]['y']}
        elif direction == RIGHT:
            newHead = {'x': wormCoords[HEAD]['x'] + 1, 'y': wormCoords[HEAD]['y']}


        # check if the worm has hit itself or the edge
        if worm2Coords[HEAD]['x'] == -1 or worm2Coords[HEAD]['x'] == CELLWIDTH or worm2Coords[HEAD]['y'] == -1 or \
                worm2Coords[HEAD]['y'] == CELLHEIGHT:
            return  # game over
        for wormBody2 in worm2Coords[1:]:
            if wormBody2['x'] == worm2Coords[HEAD]['x'] and wormBody2['y'] == worm2Coords[HEAD]['y']:
                return  # game over
        for wormBody in wormCoords[1:]:
            if wormBody['x'] == worm2Coords[HEAD]['x'] and wormBody['y'] == worm2Coords[HEAD]['y']:
                return  # game over


                    # move the worm by adding a segment in the direction it is moving
        if direction2 == UP:
            newHead2 = {'x': worm2Coords[HEAD]['x'], 'y': worm2Coords[HEAD]['y'] - 1}
        elif direction2 == DOWN:
            newHead2 = {'x': worm2Coords[HEAD]['x'], 'y': worm2Coords[HEAD]['y'] + 1}
        elif direction2 == LEFT:
            newHead2 = {'x': worm2Coords[HEAD]['x'] - 1, 'y': worm2Coords[HEAD]['y']}
        elif direction2 == RIGHT:
            newHead2 = {'x': worm2Coords[HEAD]['x'] + 1, 'y': worm2Coords[HEAD]['y']}

        wormCoords.insert(0, newHead)   #have already removed the last segment
        worm2Coords.insert(0, newHead2)   #have already removed the last segment

        DISPLAYSURF.fill(BGCOLOR)

        drawGrid()
        for num, bullet in enumerate(bullets):
           bullets[num] = drawBullet(bullet)
        for stone in stones:
            drawStone(stone)
        drawWorm(wormCoords)
        drawWorm(worm2Coords)
        for apple in apples:
            drawApple(apple)
        drawScore(len(wormCoords) - 3, len(worm2Coords) - 3)
        pygame.display.update()
        FPSCLOCK.tick(FPS)

def drawPressKeyMsg():
    pressKeySurf = BASICFONT.render('Press a key to play.', True, YELLOW)
    pressKeyRect = pressKeySurf.get_rect()
    pressKeyRect.topleft = (WINDOWWIDTH - 200, WINDOWHEIGHT - 30)
    DISPLAYSURF.blit(pressKeySurf, pressKeyRect)

def checkForKeyPress():
    if len(pygame.event.get(QUIT)) > 0:
        terminate()

    keyUpEvents = pygame.event.get(KEYUP)
    if len(keyUpEvents) == 0:
        return None
    if keyUpEvents[0].key == K_ESCAPE:
        terminate()
    return keyUpEvents[0].key


def showStartScreen():
    titleFont = pygame.font.Font('freesansbold.ttf', 100)
    titleSurf1 = titleFont.render('Snakes', True, DARKGRAY, YELLOW)
    titleSurf2 = titleFont.render('Snakes', True, WHITE)

    degrees1 = 0
    degrees2 = 0
    while True:
        DISPLAYSURF.fill(BGCOLOR)
        rotatedSurf1 = pygame.transform.rotate(titleSurf1, degrees1)
        rotatedRect1 = rotatedSurf1.get_rect()
        rotatedRect1.center = (math.floor(WINDOWWIDTH / 2), math.floor(WINDOWHEIGHT / 2))
        DISPLAYSURF.blit(rotatedSurf1, rotatedRect1)

        rotatedSurf2 = pygame.transform.rotate(titleSurf2, degrees2)
        rotatedRect2 = rotatedSurf2.get_rect()
        rotatedRect2.center = (math.floor(WINDOWWIDTH / 2), math.floor(WINDOWHEIGHT / 2))
        DISPLAYSURF.blit(rotatedSurf2, rotatedRect2)

        drawPressKeyMsg()

        if checkForKeyPress():
            pygame.event.get() # clear event queue
            return
        pygame.display.update()
        FPSCLOCK.tick(FPS)
        degrees1 += 3 # rotate by 3 degrees each frame
        degrees2 += 7 # rotate by 7 degrees each frame


def terminate():
    pygame.quit()
    sys.exit()


def getRandomLocation():
    return {'x': random.randint(0, CELLWIDTH - 1), 'y': random.randint(0, CELLHEIGHT - 1)}


def showGameOverScreen():
    gameOverFont = pygame.font.Font('freesansbold.ttf', 150)
    gameSurf = gameOverFont.render('Game', True, WHITE)
    overSurf = gameOverFont.render('Over', True, WHITE)
    gameRect = gameSurf.get_rect()
    overRect = overSurf.get_rect()
    gameRect.midtop = (math.floor(WINDOWWIDTH / 2), 10)
    overRect.midtop = (math.floor(WINDOWWIDTH / 2), gameRect.height + 10 + 25)

    DISPLAYSURF.blit(gameSurf, gameRect)
    DISPLAYSURF.blit(overSurf, overRect)
    drawPressKeyMsg()
    pygame.display.update()
    pygame.time.wait(500)
    checkForKeyPress() # clear out any key presses in the event queue

    while True:
        if checkForKeyPress():
            pygame.event.get() # clear event queue
            return

def drawScore(score, score2):
    scoreSurf = BASICFONT.render('Score 1: %s Score 2: %s' % (score, score2), True, WHITE)
    scoreRect = scoreSurf.get_rect()
    scoreRect.topleft = (WINDOWWIDTH - 240, 10)
    DISPLAYSURF.blit(scoreSurf, scoreRect)


def drawWorm(wormCoords):
    for coord in wormCoords:
        x = coord['x'] * CELLSIZE
        y = coord['y'] * CELLSIZE
        wormSegmentRect = pygame.Rect(x, y, CELLSIZE, CELLSIZE)
        pygame.draw.rect(DISPLAYSURF, DARKGREEN, wormSegmentRect)
        wormInnerSegmentRect = pygame.Rect(x + 4, y + 4, CELLSIZE - 8, CELLSIZE - 8)
        pygame.draw.rect(DISPLAYSURF, GREEN, wormInnerSegmentRect)


def drawApple(coord):
    x = coord['x'] * CELLSIZE
    y = coord['y'] * CELLSIZE
    xcenter = coord['x'] * CELLSIZE + math.floor(CELLSIZE/2)
    ycenter = coord['y'] * CELLSIZE+ math.floor(CELLSIZE/2)
    #appleRect = pygame.Rect(x, y, CELLSIZE, CELLSIZE)
    #pygame.draw.rect(DISPLAYSURF, RED, appleRect)
    pygame.draw.circle(DISPLAYSURF, RED,(xcenter,ycenter),RADIUS)

def drawStone(stone):
    x = stone['x'] * CELLSIZE
    y = stone['y'] * CELLSIZE
    xcenter = stone['x'] * CELLSIZE + math.floor(CELLSIZE/2)
    ycenter = stone['y'] * CELLSIZE+ math.floor(CELLSIZE/2)
    #appleRect = pygame.Rect(x, y, CELLSIZE, CELLSIZE)
    #pygame.draw.rect(DISPLAYSURF, RED, appleRect)
    pygame.draw.circle(DISPLAYSURF, DARKGRAY,(xcenter,ycenter),RADIUS)

def drawBullet(bullet):
    direction = bullet.direction
    if direction == UP:
        bullet.position['y'] -= 1
    elif direction == DOWN:
        bullet.position['y'] += 1
    elif direction == LEFT:
        bullet.position['x'] -= 1
    elif direction == RIGHT:
        bullet.position['x'] += 1
    xcenter = bullet.position["x"] * CELLSIZE + math.floor(CELLSIZE / 2)
    ycenter = bullet.position["y"] * CELLSIZE + math.floor(CELLSIZE / 2)
    pygame.draw.circle(DISPLAYSURF, YELLOW,(int(xcenter),int(ycenter)),int(RADIUS))

    return bullet


def drawGrid():
    for x in range(0, WINDOWWIDTH, CELLSIZE): # draw vertical lines
        pygame.draw.line(DISPLAYSURF, DARKGRAY, (x, 0), (x, WINDOWHEIGHT))
    for y in range(0, WINDOWHEIGHT, CELLSIZE): # draw horizontal lines
        pygame.draw.line(DISPLAYSURF, DARKGRAY, (0, y), (WINDOWWIDTH, y))


if __name__ == '__main__':
    main()