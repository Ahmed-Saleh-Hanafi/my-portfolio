
# Definition for singly-linked list.
# class ListNode:
#     def __init__(self, val=0, next=None):
#         self.val = val
#         self.next = next
class Solution:
    def getDecimalValue(self, head: Optional[ListNode]) -> int:
        bin_num = []
        node = head
        while node:
            bin_num.append(node.val)
            node = node.next
        bin_num.reverse()

        dec_num = 0
        arr = [ 2**i for i in range(len(bin_num))]
        for idx, val in enumerate(arr):
            dec_num += val * bin_num[idx] 

        return dec_num
        # Time Complexity: O(n)
        # Space Complexity: O(n)