using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Makes it easy to configure the content of a SelectionScreen
namespace Castle.EventNodes.Customization
{
    class ChoiceBuilder<T>
    {
        public ChoiceBuilder()
        {
        }
        public ChoiceBuilder(IEnumerable<T> choices)
        {
            foreach (T item in choices)
            {
                this.put(item.ToString(), item);
            }
        }
        public ChoiceBuilder(Dictionary<String, T> labeledChoices)
        {
            foreach (KeyValuePair<String, T> entry in labeledChoices)
            {
                this.put(entry.Key, entry.Value);
            }
        }
        public ChoiceBuilder(List<KeyValuePair<String, T>> labeledChoices)
        {
            foreach (KeyValuePair<String, T> entry in labeledChoices)
            {
                this.put(entry.Key, entry.Value);
            }
        }
        public ChoiceBuilder<T> And(ChoiceBuilder<T> other)
        {
            ChoiceBuilder<T> union = new ChoiceBuilder<T>(this.choices);
            foreach (KeyValuePair<String, T> entry in other.choices)
            {
                union.put(entry.Key, entry.Value);
            }
            return union;
        }
        public ChoiceBuilder<T> And(IEnumerable<T> choices)
        {
            return this.And(new ChoiceBuilder<T>(choices));
        }
        public ChoiceBuilder<T> And(Dictionary<String, T> choices)
        {
            return this.And(new ChoiceBuilder<T>(choices));
        }
        public ChoiceBuilder<T> And(String key, T value)
        {
            return this.And(new Dictionary<String, T>(){{key, value}});
        }
        private void put(String key, T value)
        {
            this.choices.Add(new KeyValuePair<string, T>(key, value));
        }
        public List<KeyValuePair<String, T>> Build()
        {
            return this.choices;
        }
        private List<KeyValuePair<String, T>> choices = new List<KeyValuePair<String, T>>();
    }
}
