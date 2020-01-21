# check if the worm has hit itself or the edge
        if worm2Coords[HEAD2]['x'] == -1 or worm2Coords[HEAD2]['x'] == CELLWIDTH or worm2Coords[HEAD2]['y'] == -1 or worm2Coords[HEAD2]['y'] == CELLHEIGHT:
            return # game over
        for wormBody2 in worm2Coords[1:]:
            if wormBody2['x'] == worm2Coords[HEAD2]['x'] and wormBody2['y'] == worm2Coords[HEAD2]['y']:
                return # game over

        # check if worm has eaten an apply
        if worm2Coords[HEAD2]['x'] == apple['x'] and worm2Coords[HEAD2]['y'] == apple['y']:
            # don't remove worm's tail segment
            apple = getRandomLocation() # set a new apple somewhere
        elif worm2Coords[HEAD2]['x'] == apple2['x'] and worm2Coords[HEAD2]['y'] == apple2['y']:
            # don't remove worm's tail segment
            apple2 = getRandomLocation()  # set a new apple somewhere
        elif worm2Coords[HEAD2]['x'] == apple3['x'] and worm2Coords[HEAD2]['y'] == apple3['y']:
            # don't remove worm's tail segment
            apple3 = getRandomLocation()  # set a new apple somewhere
        else:
            del worm2Coords[-1]  # remove worm's tail segment

        # move the worm by adding a segment in the direction it is moving
        if direction == UP:
            newHead2 = {'x': worm2Coords[HEAD2]['x'], 'y': worm2Coords[HEAD2]['y'] - 1}
        elif direction == DOWN:
            newHead2 = {'x': worm2Coords[HEAD2]['x'], 'y': worm2Coords[HEAD2]['y'] + 1}
        elif direction == LEFT:
            newHead2 = {'x': worm2Coords[HEAD2]['x'] - 1, 'y': worm2Coords[HEAD2]['y']}
        elif direction == RIGHT:
            newHead2 = {'x': worm2Coords[HEAD2]['x'] + 1, 'y': worm2Coords[HEAD2]['y']}