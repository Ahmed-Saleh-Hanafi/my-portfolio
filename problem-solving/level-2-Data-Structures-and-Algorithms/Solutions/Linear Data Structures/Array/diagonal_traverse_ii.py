
class Solution:
    def findDiagonalOrder(self, nums: list[list[int]]) -> list[int]:
        """_summary_
        Given a 2D array nums, return all elements of nums in diagonal order.
        Time Complexity: O(N log N)
        Space Complexity: O(N)
        """
        index = []
        for row in range( len (nums)):
            for col in range (len(nums[row])):
                index.append((row+col,col, row))
                
        index.sort()
        ans = []
        for _ , col , row in index :
            ans.append(nums[row][col])
        return ans

# Optimized Approach
from collections import deque
class Solution:
    def findDiagonalOrder(self, nums: list[list[int]]) -> list[int]:
        """_summary_
        Given a 2D array nums, return all elements of nums in diagonal order.
        Time Complexity: O(N)
        Space Complexity: O(N)
        """
        queue = deque([(0,0)])
        ans = []
        while queue:
            row , col = queue.popleft()
            ans.append(nums[row][col])

            if col == 0 and row + 1 < len(nums):
                queue.append((row+1, 0))
            
            if col + 1 < len (nums[row]):
                queue.append((row, col+1))

        return ans