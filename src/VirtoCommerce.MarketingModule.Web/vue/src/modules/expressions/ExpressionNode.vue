<template>
  <div
    class="mk-expr"
    :class="depth === 0 ? 'mk-expr--block' : 'mk-expr--item'"
    :data-expression="node.id"
  >
    <div class="mk-expr__head">
      <div class="mk-expr__line">
        <template
          v-for="(part, i) in def.parts"
          :key="i"
        >
          <span
            v-if="typeof part === 'string'"
            class="mk-expr__text"
            >{{ part }}</span
          >
          <ExpressionField
            v-else
            :node="node"
            :part="part"
            :disabled="disabled"
          />
        </template>
      </div>
      <VcButton
        v-if="removable && !disabled"
        class="mk-expr__remove"
        variant="ghost"
        size="icon-sm"
        icon="lucide-x"
        :aria-label="$t('MARKETING.EXPRESSIONS.REMOVE')"
        @click="$emit('remove')"
      />
    </div>

    <!-- excluded categories/products of a catalog or cart condition -->
    <div
      v-if="hasExcluding"
      class="mk-expr__line mk-expr__excluding"
    >
      <span class="mk-expr__text">{{ $t("MARKETING.EXPRESSIONS.EXCLUDING") }}</span>
      <ExpressionField
        v-if="Array.isArray(node.excludingCategoryIds)"
        :node="node"
        :part="{ kind: 'ids', path: 'excludingCategoryIds', of: 'category', placeholder: $t('MARKETING.EXPRESSIONS.EXCLUDE_CATEGORIES') }"
        :disabled="disabled"
      />
      <ExpressionField
        v-if="Array.isArray(node.excludingProductIds)"
        :node="node"
        :part="{ kind: 'ids', path: 'excludingProductIds', of: 'product', placeholder: $t('MARKETING.EXPRESSIONS.EXCLUDE_PRODUCTS') }"
        :disabled="disabled"
      />
    </div>

    <div
      v-if="children.length"
      class="mk-expr__children"
    >
      <ExpressionNode
        v-for="(child, i) in children"
        :key="keyOf(child)"
        :node="child"
        :depth="depth + 1"
        :disabled="disabled"
        removable
        @remove="node.children!.splice(i, 1)"
      />
    </div>

    <VcHint
      v-if="error"
      class="mk-expr__error"
      error
      >{{ error }}</VcHint
    >

    <VcDropdown
      v-if="addable.length && !disabled"
      v-model="menuOpen"
      :items="addable"
      :item-text="(x: AddItem) => x.label"
      floating
      close-on-select
      max-height="360"
      @item-click="add"
    >
      <template #trigger="{ toggle }">
        <VcButton
          class="mk-expr__add"
          variant="link"
          size="sm"
          icon="lucide-plus"
          @click="toggle"
          >{{ def.addLabel || $t("MARKETING.EXPRESSIONS.ADD") }}</VcButton
        >
      </template>
    </VcDropdown>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { VcButton, VcDropdown, VcHint } from "@vc-shell/framework/ui";
import ExpressionField from "./ExpressionField.vue";
import { defOf } from "./defs";

type Node = Record<string, unknown> & { id?: string; children?: Node[]; availableChildren?: Node[] };
interface AddItem {
  label: string;
  group?: string;
  prototype: Node;
}

defineOptions({ name: "ExpressionNode" });
const props = defineProps<{ node: Node; depth?: number; removable?: boolean; disabled?: boolean }>();
defineEmits<{ remove: [] }>();

const depth = computed(() => props.depth ?? 0);
const def = computed(() => defOf(props.node));
const children = computed(() => props.node.children ?? []);
const hasExcluding = computed(() => Array.isArray(props.node.excludingCategoryIds) || Array.isArray(props.node.excludingProductIds));
const error = computed(() => (def.value.requireChild && !children.value.length ? def.value.requireChild : undefined));

// the "Add ..." menu lists the prototypes the API sends with the node (availableChildren), grouped where the editor groups them
const addable = computed<AddItem[]>(() =>
  (props.node.availableChildren ?? [])
    .map((prototype) => {
      const d = defOf(prototype);
      return { label: d.group ? `${d.group}: ${d.label}` : (d.label ?? String(prototype.id)), group: d.group, prototype };
    })
    .sort((a, b) => (a.group ? a.label.localeCompare(b.label) : 0)),
);
const menuOpen = ref(false);

// a new child is a copy of the server's prototype, so it carries exactly the server's field set
function add(item: AddItem) {
  (props.node.children ??= []).push(JSON.parse(JSON.stringify(item.prototype)) as Node);
}

// stable keys without writing anything onto the API objects
const keys = new WeakMap<object, number>();
let seq = 0;
const keyOf = (o: object) => keys.get(o) ?? (keys.set(o, ++seq), seq);
</script>

<style lang="scss">
.mk-expr {
  font-size: 14px;
  line-height: 20px;
  color: var(--neutrals-700);

  &--block {
    padding: 14px 16px;
    & + & {
      border-top: 1px solid var(--neutrals-200);
    }
    > .mk-expr__head .mk-expr__text {
      font-weight: 600;
      color: var(--neutrals-900);
    }
  }

  /* the sentence wraps; the remove button stays at the end of its first line */
  &__head {
    display: flex;
    align-items: flex-start;
    gap: 8px;
  }

  &__line {
    flex: 1 1 auto;
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: 6px 8px;
    min-height: 32px;
  }

  &__excluding {
    margin-top: 6px;
  }

  &__children {
    margin: 8px 0 4px 6px;
    padding-left: 14px;
    border-left: 2px solid var(--neutrals-200);
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

  &__remove {
    flex: none;
    margin-top: 4px;
  }

  &__error {
    margin-top: 6px;
  }

  &__add {
    margin-top: 6px;
  }
}
</style>
