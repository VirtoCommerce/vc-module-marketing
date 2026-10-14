<template>
  <!-- the root (PromotionConditionAndRewardTree / DynamicContentConditionTree) is fixed: its blocks are edited, not added -->
  <div class="mk-expr-tree">
    <ExpressionNode
      v-for="(block, i) in tree?.children ?? []"
      :key="i"
      :node="block"
      :disabled="disabled"
    />
  </div>
</template>

<script setup lang="ts">
import ExpressionNode from "./ExpressionNode.vue";

defineProps<{ tree?: { children?: Record<string, unknown>[] } | null; disabled?: boolean }>();
</script>
